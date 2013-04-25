using System.Linq;
using Elapsus.Usms.Data.SqlServer;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using NUnit.Framework;
using Test.Shared.Usms.Gateway;
using TypeMock;

namespace Test.Usms.Gateway.Logging
{
	[TestFixture]
	public class Test_LogProvider_SqlServer
	{
		[SetUp]
		public void SetUp()
		{
			MockService.MockSqlServerLogProvider();
			MockService.MockFakeSmsProvider();
		}

		/// <summary>
		/// Send an immediate sms and verify it is correctly inserted 
		/// into the database
		/// </summary>
		[Test, VerifyMocks]
		public void InsertToLog()
		{
			ISmsc smsc;
			ISms sms;
			SmsLog loggedSms;
			ISmsStatus status;
			SmsLogDataContext db;

			db = new SmsLogDataContext();

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Test Message";

			status = smsc.SendImmediate(sms);
			Assert.AreEqual(EnMtStatus.Accepted, status.Status);

			var smsLog = from log in db.SmsLogs
			             where log.SmsLogId == sms.ReferenceId
			             select log;

			loggedSms = smsLog.Single();
			Helpers.CompareSms(sms, loggedSms);

			Assert.IsNotNull(loggedSms.AcceptedOn);
			Assert.IsNotNull(loggedSms.ForwardedOn);
			Assert.IsNull(loggedSms.SmsLogRecipients[0].NotifiedOn);
			Assert.AreEqual(0, loggedSms.Failures);
			Assert.AreEqual(1, loggedSms.Successes);
			Assert.IsNull(loggedSms.ErrorCode);
			Assert.IsNull(loggedSms.ErrorMessage);
		}

		/// <summary>
		/// Send an immediate sms and verify it is correctly inserted 
		/// into the database
		/// </summary>
		[Test, VerifyMocks]
		public void InsertMultipleToLog()
		{
			ISmsc smsc;
			ISms sms;
			SmsLog loggedSms;
			ISmsStatusCollection status;
			SmsLogDataContext db;

			db = new SmsLogDataContext();

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.AddRecipient(TestConstants.Default_Recipient_3);
			sms.Message = "Test Message";

			status = smsc.SendMultipleImmediate(sms);
			Assert.IsTrue(status.Successful);
			foreach (ISmsStatus stat in status)
				Assert.AreEqual(EnMtStatus.Accepted, stat.Status);

			var smsLog = from log in db.SmsLogs
									 where log.SmsLogId == sms.ReferenceId
									 select log;

			loggedSms = smsLog.Single();
			Helpers.CompareSms(sms, loggedSms);

			Assert.IsNotNull(loggedSms.AcceptedOn);
			Assert.IsNotNull(loggedSms.ForwardedOn);
			Assert.AreEqual(0, loggedSms.Failures);
			Assert.AreEqual(3, loggedSms.Successes);
			Assert.IsNull(loggedSms.ErrorCode);
			Assert.IsNull(loggedSms.ErrorMessage);

			foreach (SmsLogRecipient recipient in loggedSms.SmsLogRecipients)
			{
				Assert.IsNull(recipient.NotifiedOn);
				Assert.IsNull(recipient.NotificationCode);
				Assert.IsNull(recipient.NotificationMessage);
			}
		}

		/// <summary>
		/// Send an immediate sms and verify it is correctly inserted 
		/// into the database.
		/// A forward error is simulated
		/// </summary>
		[Test, VerifyMocks]
		public void InsertToLog_WithFailure()
		{
			ISmsc smsc;
			ISms sms;
			SmsLog loggedSms;
			ISmsStatus status;
			SmsLogDataContext db;

			db = new SmsLogDataContext();

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Test Message";
			sms.CustomProperties["FailCode"] = "-15";

			status = smsc.SendImmediate(sms);
			Assert.AreEqual(EnMtStatus.Failure, status.Status);

			var smsLog = from log in db.SmsLogs
									 where log.SmsLogId == sms.ReferenceId
									 select log;

			loggedSms = smsLog.Single();
			Helpers.CompareSms(sms, loggedSms);

			Assert.IsNotNull(loggedSms.AcceptedOn);
			Assert.IsNotNull(loggedSms.ForwardedOn);
			Assert.IsNotNull(loggedSms.SmsLogRecipients[0].NotifiedOn);
			Assert.AreEqual(1, loggedSms.Failures);
			Assert.AreEqual(0, loggedSms.Successes);
			Assert.AreEqual(-15, loggedSms.SmsLogRecipients[0].NotificationCode);
			Assert.IsNotNull(loggedSms.SmsLogRecipients[0].NotificationMessage);
		}

		/// <summary>
		/// Send an immediate sms and verify it is correctly inserted 
		/// into the database
		/// A forward error is simulated
		/// </summary>
		[Test, VerifyMocks]
		public void InsertMultipleToLog_WitFailure()
		{
			ISmsc smsc;
			ISms sms;
			SmsLog loggedSms;
			ISmsStatusCollection status;
			SmsLogDataContext db;

			db = new SmsLogDataContext();

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.AddRecipient(TestConstants.Default_Recipient_3);
			sms.Message = "Test Message";
			sms.CustomProperties["FailCode"] = "-15";

			status = smsc.SendMultipleImmediate(sms);
			Assert.IsFalse(status.Successful);
			foreach (ISmsStatus stat in status)
				Assert.AreEqual(EnMtStatus.Failure, stat.Status);

			var smsLog = from log in db.SmsLogs
									 where log.SmsLogId == sms.ReferenceId
									 select log;

			loggedSms = smsLog.Single();
			Helpers.CompareSms(sms, loggedSms);

			Assert.IsNotNull(loggedSms.AcceptedOn);
			Assert.IsNotNull(loggedSms.ForwardedOn);
			Assert.AreEqual(3, loggedSms.Failures);
			Assert.AreEqual(0, loggedSms.Successes);
			Assert.IsNull(loggedSms.ErrorCode);
			Assert.IsNull(loggedSms.ErrorMessage);

			foreach (SmsLogRecipient recipient in loggedSms.SmsLogRecipients)
			{
				Assert.IsNotNull(recipient.NotifiedOn);
				Assert.IsNotNull(recipient.NotificationMessage);
				Assert.AreEqual(-15, recipient.NotificationCode);
			}
		}
	}
}