using System.Threading;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;
using Elapsus.Usms.Gateway.Interface.Providers.Queue;
using Elapsus.Usms.Logging.SqlServer;
using NUnit.Framework;
using Test.Shared.Usms.Gateway;
using TypeMock;

namespace Test.Usms.Gateway.Queuing
{
	[TestFixture]
	public class Test_QueueProvider_SqlServer
	{
		[SetUp]
		public void SetUp()
		{
			MockManager.Init();
			MockService.MockQueueProvider();
			MockService.MockSqlServerLogProvider();
			EmptyQueue();
		}

		/// <summary>
		/// Attempt to dequeue from an empty queue
		/// Expected for the returned message to be null
		/// </summary>
		[Test, VerifyMocks]
		public void DequeueOnEmptyQueue()
		{
			IReadOnlySms sms;
			ISmsc smsc;
			IDequeuer dequeuer;

			smsc = SmsFactory.GetSmsc();
			dequeuer = smsc.GetDequeuer();

			// Verify the queue is empty
			Assert.IsTrue(smsc.IsQueueEmpty());

			// Retrieve a message from the queue
			sms = dequeuer.Dequeue();
			Assert.IsNull(sms);
		}

		/// <summary>
		/// Enqueue a message on an empty queue and verify the same
		/// message is dequeued
		/// </summary>
		[Test, VerifyMocks]
		public void EnqueueAndDequeue()
		{
			ISms sms;
			IReadOnlySms enqueuedSms;
			IDequeuer dequeuer;
			ISmsc smsc;
			ISmsProvider provider;

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("Fake");

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Test Message";
			sms.Provider = provider;

			// Verify the queue is empty
			Assert.IsTrue(smsc.IsQueueEmpty());

			// Insert a new message to the queue
			smsc.Enqueue(sms);

			// Verify the queue is not empty
			Assert.IsFalse(smsc.IsQueueEmpty());

			// Retrieve a message from the queue
			dequeuer = smsc.GetDequeuer();
			enqueuedSms = dequeuer.Dequeue();
			dequeuer.DequeueCommit(true);

			Helpers.CompareSms(sms, enqueuedSms);
		}

		/// <summary>
		/// Insert a message in the queue, dequeue the message
		/// but mark the transactions as failed.
		/// The message is expected to be still in the queue
		/// </summary>
		[Test]
		public void DequeueRollback()
		{
			ISms sms;
			IReadOnlySms enqueuedSms;
			IDequeuer dequeuer;
			ISmsc smsc;
			ISmsProvider provider;

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("Fake");

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Test Message";
			sms.Provider = provider;

			// Verify the queue is empty
			Assert.IsTrue(smsc.IsQueueEmpty());

			// Insert a new message to the queue
			smsc.Enqueue(sms);

			// Verify the queue is not empty
			Assert.IsFalse(smsc.IsQueueEmpty());

			// Retrieve a message from the queue
			dequeuer = smsc.GetDequeuer();
			enqueuedSms = dequeuer.Dequeue();
			dequeuer.DequeueCommit(false);

			// Verify the queue is still not empty
			Assert.IsFalse(smsc.IsQueueEmpty());

			Helpers.CompareSms(sms, enqueuedSms);

			// Retrieve a message from the queue again
			dequeuer = smsc.GetDequeuer();
			enqueuedSms = dequeuer.Dequeue();
			dequeuer.DequeueCommit(true);

			// Verify the queue is empty
			Assert.IsTrue(smsc.IsQueueEmpty());

			Helpers.CompareSms(sms, enqueuedSms);
		}

		/// <summary>
		/// Enqueue and dequeue 5 messages
		/// </summary>
		[Test]
		public void EnqueueAndDequeueMultipleMessages()
		{
			ISmsc smsc;
			ISms[] messages;

			messages = new ISms[5];

			smsc = SmsFactory.GetSmsc();

			// Verify the queue is empty
			Assert.IsTrue(smsc.IsQueueEmpty());

			for (int count = 0; count < 5; ++count)
			{
				messages[count] = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
				messages[count].Sender = TestConstants.Default_Sender;
				messages[count].AddRecipient(TestConstants.Default_Recipient);
				messages[count].Message = string.Format("Message {0}", count + 1);
				smsc.Enqueue(messages[count]);
			}

			// Verify the queue is not empty
			Assert.IsFalse(smsc.IsQueueEmpty());

			for (int count = 0; count < 5; ++count)
			{
				using (var dequeuer = smsc.GetDequeuer())
				{
					var enqueuedSms = dequeuer.Dequeue();
					dequeuer.DequeueCommit(true);
					Helpers.CompareSms(messages[count], enqueuedSms);
				}
			}

			// Verify the queue is empty
			Assert.IsTrue(smsc.IsQueueEmpty());
		}

		/// <summary>
		/// Create an instance of Smsc in DequeueService mode<br/>
		/// Instantiate 5 dequeuers<br/>
		/// Enqueue 20 messages<br/>
		/// Run the enqueuers<br/>
		/// Verify that messages are processed 
		/// </summary>
		[Test, VerifyMocks]
		public void RunDequeuingThreads()
		{
			IDequeueService dequeuers;
			Mock<SqlServerLogProvider> mockLogProvider;
			ISmsc smsc;
			ISms[] messages;

			mockLogProvider = MockManager.Mock<SqlServerLogProvider>(Constructor.NotMocked);
			mockLogProvider.ExpectCall("OnEnqueuement", 20);

			smsc = SmsFactory.GetSmsc();

			// Create and enqueue 20 messages
			messages = new ISms[20];
			for (int count = 0; count < 20; ++count)
			{
				messages[count] = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
				messages[count].Sender = TestConstants.Default_Sender;
				messages[count].AddRecipient(TestConstants.Default_Recipient);
				messages[count].Message = string.Format("Message {0}", count + 1);
				smsc.Enqueue(messages[count]);
			}

			mockLogProvider = MockManager.Mock<SqlServerLogProvider>(Constructor.NotMocked);
			mockLogProvider.ExpectCall("OnSubmission", 20);

			// Instantiate a dequeuer with 5 instances
			dequeuers = SmsFactory.CreateDequeueService(5);

			Assert.IsFalse(dequeuers.IsQueueEmpty());

			// Run the 5 dequeuer threads
			dequeuers.Start();

			Thread.Sleep(2000);

			// Wait until all dequeuer threads are idle
			dequeuers.Suspend();

			Assert.IsTrue(dequeuers.IsQueueEmpty());

			dequeuers.Terminate();
		}

		private static void EmptyQueue()
		{
			ISmsc smsc;
			IDequeuer dequeuer;

			smsc = SmsFactory.GetSmsc();
			dequeuer = smsc.GetDequeuer();

			while (smsc.IsQueueEmpty() == false)
			{
				dequeuer.Dequeue();
				dequeuer.DequeueCommit(true);
			}
		}
	}
}