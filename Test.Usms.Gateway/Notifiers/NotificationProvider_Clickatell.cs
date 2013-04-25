using System;
using Elapsus.Usms.Data.SqlServer;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Notification.Clickatell;
using NUnit.Framework;
using Test.Shared.Usms.Gateway;
using TypeMock;

namespace Test.Usms.Gateway.Notifiers
{
	[TestFixture]
	public class NotificationProvider_Clickatell
	{
		[SetUp]
		public void SetUp()
		{
			MockManager.Init();

			MockService.MockFakeSmsProvider();
			MockService.MockSqlServerLogProvider();
		}

		/// <summary>
		/// Simulate a delivery notification.
		/// Verify that the DeliveredOn field
		/// in the SqlServerLogProvider is
		/// updated accordingly
		/// </summary>
		[Test, VerifyMocks]
		public void NotifyDelivery()
		{
			ClickatellNotifier notifier;
			ISmsc smsc;
			ISms sms;
			ISmsStatus status;
			SmsLog smsLog;

			smsc = SmsFactory.GetSmsc();
			sms = Helpers.FillSms();

			// Send the message
			status = smsc.SendImmediate(sms);
			Assert.AreEqual(EnMtStatus.Accepted, status.Status);

			// Verify the message is in the log 
			// and the DeliveredOn field is empty
			smsLog = DataHelpers.GetSmsLog(status.ProviderReference);
			Assert.IsNotNull(smsLog);
			Helpers.CompareSms(sms, smsLog);
			Assert.IsNull(smsLog.SmsLogRecipients[0].NotifiedOn);

			// Simulate a notification
			notifier = new ClickatellNotifier();
			notifier.ProcessNotification(DateTime.Now, status.ProviderReference, status.Recipient, (int) NotificationCode.ReceivedByRecipient, null);

			// Verify the DeliveredOn field is not null
			smsLog = DataHelpers.GetSmsLog(status.ProviderReference);
			Assert.IsNotNull(smsLog.SmsLogRecipients[0].NotifiedOn);
		}

		[Test, VerifyMocks]
		public void NotifyDeliveryError()
		{
			ClickatellNotifier notifier;
			ISmsc smsc;
			ISms sms;
			ISmsStatus status;
			SmsLog smsLog;

			smsc = SmsFactory.GetSmsc();
			sms = Helpers.FillSms();

			// Send the message 
			// Send the message
			status = smsc.SendImmediate(sms);
			Assert.AreEqual(EnMtStatus.Accepted, status.Status);

			// Verify the message is in the log 
			// and the DeliveredOn field is empty
			smsLog = DataHelpers.GetSmsLog(status.ProviderReference);
			Assert.IsNotNull(smsLog);
			Helpers.CompareSms(sms, smsLog);
			Assert.IsNull(smsLog.SmsLogRecipients[0].NotifiedOn);

			notifier = new ClickatellNotifier();
			notifier.ProcessNotification(DateTime.Now, status.ProviderReference, status.Recipient, - 1, "Sample error code");

			// Verify the error is written into the log
			smsLog = DataHelpers.GetSmsLog(status.ProviderReference);
			Assert.IsNotNull(smsLog.SmsLogRecipients[0].NotifiedOn);
			Assert.AreEqual(-1, smsLog.SmsLogRecipients[0].NotificationCode);
			Assert.AreEqual("Sample error code", smsLog.SmsLogRecipients[0].NotificationMessage);
		}
	}
}
