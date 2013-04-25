using System;
using Elapsus.Usms.Data.SqlServer;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;
using Elapsus.Usms.Gateway.Notification.Clickatell;
using Elapsus.Usms.Logging.SqlServer;
using NUnit.Framework;
using Test.Shared.Usms.Gateway;
using Test.Shared.Usms.Gateway.Web;
using TypeMock;

namespace Test.Usms.Gateway.Notifiers
{
	[TestFixture]
	public class HttpHandler_Clickatell : AspnetHostBase
	{
		public HttpHandler_Clickatell() : base("/ClickatellNotifier", Environment.CurrentDirectory)
		{
		}

		/// <summary>
		/// Send an sms, then simulate a delivery notification.
		/// Verofy that the delivery notification status is
		/// set into the database
		/// </summary>
		[Test, VerifyMocks]
		public void NotifyDelivery()
		{
			ISms sms;
			ISmsc smsc;
			ISmsStatus status;
			ISmsProvider provider;
			SmsLog smsLog;
			SmsLogRecipient recipient;

			smsc = SmsFactory.GetSmsc();
			sms = Helpers.FillSms();

			provider = smsc.SmsProviders.Lookup("Fake");
			sms.Provider = provider;

			status = smsc.SendImmediate(sms);
			Assert.AreEqual(EnMtStatus.Accepted, status.Status);

			Host.CreateRequest(
				"ClickatellNotifierHandler.ashx",
				string.Format(
					"apiMsgId={0}&cliMsgId={1}&timestamp={2}&to={3}&status={4}",
					status.ProviderReference,
					sms.ReferenceId,
					DateTime.Now.ToFileTimeUtc(),
					sms.Recipients[0],
					(int) NotificationCode.ReceivedByRecipient
					));

			smsLog = DataHelpers.GetSmsLog(status.ProviderReference);
			recipient = smsLog.SmsLogRecipients[0];

			Assert.IsNotNull(smsLog);
			Helpers.CompareSms(sms, smsLog);

			Assert.IsNotNull(recipient.NotifiedOn);
			Assert.AreEqual(NotificationCode.ReceivedByRecipient, (NotificationCode)recipient.NotificationCode);
			Assert.AreEqual(EnRecipientStatus.Delivered, (EnRecipientStatus)recipient.Status);
			Assert.AreEqual(0, smsLog.Failures);
		}

		[Test, VerifyMocks]
		public void NotifyDeliveryError()
		{
			ISms sms;
			ISmsc smsc;
			ISmsStatus status;
			SmsLog smsLog;
			SmsLogRecipient recipient;

			smsc = SmsFactory.GetSmsc();
			sms = Helpers.FillSms();

			status = smsc.SendImmediate(sms);
			Assert.AreEqual(EnMtStatus.Accepted, status.Status);

			Host.CreateRequest(
				"ClickatellNotifierHandler.ashx",
				string.Format(
					"cliMsgId={0}&timestamp={1}&to={2}&status={3}",
					status.ProviderReference,
					DateTime.Now.ToFileTimeUtc(),
					sms.Recipients[0],
					(int) NotificationCode.ErrorDeliveringMessage
					));

			smsLog = DataHelpers.GetSmsLog(status.ProviderReference);
			recipient = smsLog.SmsLogRecipients[0];

			Assert.IsNotNull(smsLog);
			Helpers.CompareSms(sms, smsLog);

			Assert.IsNotNull(recipient.NotifiedOn);
			Assert.AreEqual(NotificationCode.ErrorDeliveringMessage, (NotificationCode)recipient.NotificationCode);
			Assert.AreEqual(EnRecipientStatus.DeliveryFailure, (EnRecipientStatus)recipient.Status);
			Assert.AreEqual(1, smsLog.Failures);
		}

		[Test, VerifyMocks]
		public void NotifyStatusUpdate()
		{
			ISms sms;
			ISmsc smsc;
			ISmsStatus status;
			SmsLog smsLog;
			SmsLogRecipient recipient;

			smsc = SmsFactory.GetSmsc();
			sms = Helpers.FillSms();

			status = smsc.SendImmediate(sms);
			Assert.AreEqual(EnMtStatus.Accepted, status.Status);

			// Simulate a MessageQueued event
			Host.CreateRequest(
				"ClickatellNotifierHandler.ashx",
				string.Format(
					"cliMsgId={0}&timestamp={1}&to={2}&status={3}",
					status.ProviderReference,
					DateTime.Now.ToFileTimeUtc(),
					sms.Recipients[0],
					(int) NotificationCode.MessageQueued
					));

			smsLog = DataHelpers.GetSmsLog(status.ProviderReference);
			recipient = smsLog.SmsLogRecipients[0];

			Assert.IsNotNull(smsLog);
			Helpers.CompareSms(sms, smsLog);

			Assert.IsNotNull(recipient.NotifiedOn);
			Assert.AreEqual(NotificationCode.MessageQueued, (NotificationCode)recipient.NotificationCode);
			Assert.AreEqual(EnRecipientStatus.Submitted, (EnRecipientStatus)recipient.Status);
			Assert.AreEqual(0, smsLog.Failures);

			// Simulate a DeliveredToGateway event
			Host.CreateRequest(
				"ClickatellNotifierHandler.ashx",
				string.Format(
					"cliMsgId={0}&timestamp={1}&to={2}&status={3}",
					status.ProviderReference,
					DateTime.Now.ToFileTimeUtc(),
					sms.Recipients[0],
					(int)NotificationCode.DeliveredToGateway
					));

			smsLog = DataHelpers.GetSmsLog(status.ProviderReference);
			recipient = smsLog.SmsLogRecipients[0];

			Assert.IsNotNull(recipient.NotifiedOn);
			Assert.AreEqual(NotificationCode.DeliveredToGateway, (NotificationCode)recipient.NotificationCode);
			Assert.AreEqual(EnRecipientStatus.Delivered, (EnRecipientStatus)recipient.Status);
			Assert.AreEqual(0, smsLog.Failures);

		}
	}
}
