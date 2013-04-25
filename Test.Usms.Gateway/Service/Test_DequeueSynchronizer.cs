using Elapsus.Usms.Gateway.Engine;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers.Queue;
using NUnit.Framework;
using Test.Shared.Usms.Gateway;
using TypeMock;

namespace Test.Usms.Gateway.Service
{
	[TestFixture]
	public class Test_DequeueSynchronizer
	{
		[SetUp]
		public void SetUp()
		{
			MockManager.Init();
			MockService.MockQueueProvider();	
		}

		[Test]
		public void Start_1_Thread()
		{
			DequeueSynchronizer dequeueSynchronizer;
			ISmsc smsc;
			IQueueHandler queueHandler;

			smsc = SmsFactory.GetSmsc();
			queueHandler = (IQueueHandler) ObjectState.GetField(smsc, "_defaultQueueHandler");

			dequeueSynchronizer = new DequeueSynchronizer(1, queueHandler, Forwarder);

			Assert.AreEqual(0, dequeueSynchronizer.RunningWorkers);

			dequeueSynchronizer.ResumeWorkers();

			Assert.AreEqual(1, dequeueSynchronizer.RunningWorkers);

			dequeueSynchronizer.SuspendWorkers();

			Assert.AreEqual(0, dequeueSynchronizer.RunningWorkers);

			dequeueSynchronizer.TerminateWorkers();
		}

		[Test]
		public void Start_5_Threads()
		{
			DequeueSynchronizer dequeueSynchronizer;
			ISmsc smsc;
			IQueueHandler queueHandler;

			smsc = SmsFactory.GetSmsc();
			queueHandler = (IQueueHandler) ObjectState.GetField(smsc, "_defaultQueueHandler");

			dequeueSynchronizer = new DequeueSynchronizer(5, queueHandler, Forwarder);

			dequeueSynchronizer.SuspendWorkers();
			dequeueSynchronizer.ResumeWorkers();

			Assert.AreEqual(5, dequeueSynchronizer.RunningWorkers);

			dequeueSynchronizer.SuspendWorkers();

			Assert.AreEqual(0, dequeueSynchronizer.RunningWorkers);

			dequeueSynchronizer.ResumeWorkers();

			Assert.AreEqual(5, dequeueSynchronizer.RunningWorkers);

			dequeueSynchronizer.TerminateWorkers();

			Assert.AreEqual(0, dequeueSynchronizer.RunningWorkers);
		}

		private static void Forwarder(IReadOnlySms sms)
		{
		}
	}
}
