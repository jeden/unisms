using System;
using System.Collections;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;
using NUnit.Framework;
using Test.Shared.Usms.Gateway;

namespace Test.Usms.Gateway.Service
{
	/// <summary>
	/// Test SMS send. All sends are mocked.
	/// </summary>
	[TestFixture]
	public class Test_SendSms
	{
		/// <summary>
		/// Test a simple text message send
		/// </summary>
		[Test]
		public void Test_SendSimpleSms()
		{
			ISms sms;
			ISmsc smsc;
			ISmsStatus smsStatus;

			smsc = SmsFactory.GetSmsc();

			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Sender = TestConstants.Default_Sender;
			sms.Message = "This is a test";

			smsStatus = smsc.SendImmediate(sms);

			Assert.AreEqual(EnMtStatus.Accepted, smsStatus.Status, "SMS not sent");
		}

		/// <summary>
		/// Try to send an SMS to 2 recipients.<br/>
		/// SendImmediate is supposed to be used for 1 single recipient,
		/// so a InvalidSmsOperationException is expected. 
		/// </summary>
		[Test]
		public void Test_SendSms_2_Recipients_UsingWrongMethod()
		{
			ISms sms;
			ISmsc smsc;

			smsc = SmsFactory.GetSmsc();

			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Sender = TestConstants.Default_Sender;
			sms.Message = "Test";

			try
			{
				smsc.SendImmediate(sms);
				Assert.Fail("Expected InvalidSmsOperationException");
			}
			catch(InvalidSmsOperationException ex)
			{
				Assert.AreEqual(EnSmsOperation.MultipleRecipients, ex.SmsOperation);
			}
		}

		/// <summary>
		/// Send an immediate SMS to 2 recipients
		/// </summary>
		[Test]
		public void Test_SendSms_2_Recipients()
		{
			ISms sms;
			ISmsc smsc;
			ISmsStatusCollection smsStatuses;

			smsc = SmsFactory.GetSmsc();

			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Sender = TestConstants.Default_Sender;
			sms.Message = "Test";

			smsStatuses = smsc.SendMultipleImmediate(sms);
			Assert.IsNotNull(smsStatuses, "SMS status collection is null");
			Assert.IsNotEmpty(smsStatuses as ICollection, "SMS status collection is empty");
			Assert.AreEqual(2, smsStatuses.Count);
			Assert.AreSame(sms, smsStatuses.Sms, "Original SMS and the instance contained in SMS Status Collection should be the same");

			Assert.AreEqual(TestConstants.Default_Recipient_Normalized, smsStatuses[0].Recipient, "Recipients not matching");
			Assert.AreEqual(TestConstants.Default_Recipient_2_Normalized, smsStatuses[1].Recipient, "Recipients not matching");

			Assert.AreEqual(EnMtStatus.Accepted, smsStatuses[0].Status);
			Assert.AreEqual(EnMtStatus.Accepted, smsStatuses[1].Status);

			Assert.IsNull(smsStatuses[0].ErrorMessage, "No error message expected");
			Assert.IsNull(smsStatuses[1].ErrorMessage, "No error message expected");
		}

		/// <summary>
		/// Verify that after sending a message the internal status is frozen.<br/>
		/// </summary>
		/// <remarks>
		/// The class instance cannot be used to send again, or to add, modify or
		/// remove recipients.<br/>
		/// This test is performed on an immediate send to a single recipient
		/// </remarks>
		[Test]
		public void Test_SendSms_ExceptionAfterFirstSend()
		{
			ISms sms;
			ISmsc smsc;
			ISmsStatus smsStatus;

			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			smsc = SmsFactory.GetSmsc();

			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Sender = TestConstants.Default_Sender;
			sms.Message = "test";

			smsStatus = smsc.SendImmediate(sms);
			Assert.AreEqual(EnMtStatus.Accepted, smsStatus.Status, "Invalid send outcome");

			try
			{
				sms.AddRecipient(TestConstants.Default_Recipient_2);
				Assert.Fail("Expected InvalidSmsOperationException exception");
			}
			catch (InvalidSmsOperationException ex)
			{
				Assert.AreEqual(EnSmsOperation.AlreadyProcessed, ex.SmsOperation, "Invalid exception type");
			}

			try
			{
				sms.Message = "New content";
				Assert.Fail("Expected InvalidSmsOperationException exception");
			}
			catch (InvalidSmsOperationException ex)
			{
				Assert.AreEqual(EnSmsOperation.AlreadyProcessed, ex.SmsOperation, "Invalid exception type");
			}

			try
			{
				smsc.SendImmediate(sms);
				Assert.Fail("Expected InvalidSmsOperationException exception");
			}
			catch (InvalidSmsOperationException ex)
			{
				Assert.AreEqual(EnSmsOperation.AlreadyProcessed, ex.SmsOperation, "Invalid exception type");
			}
		}

		/// <summary>
		/// Verify that after sending a message the internal status is frozen.<br/>
		/// </summary>
		/// <remarks>
		/// The class instance cannot be used to send again, or to add, modify or
		/// remove recipients.<br/>
		/// This test is performed on an immediate send to multiple recipients
		/// </remarks>
		[Test]
		public void Test_SendSms_ExceptionAfterFirstSend_MultipleRecipients()
		{
			ISms sms;
			ISmsc smsc;
			ISmsStatusCollection statuses;

			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			smsc = SmsFactory.GetSmsc();

			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Sender = TestConstants.Default_Sender;
			sms.Message = "test";

			statuses = smsc.SendMultipleImmediate(sms);
			Assert.IsTrue(statuses.Successful, "Invalid send outcome");

			try
			{
				sms.AddRecipient(TestConstants.Default_Recipient_2);
				Assert.Fail("Expected InvalidSmsOperationException exception");
			}
			catch (InvalidSmsOperationException ex)
			{
				Assert.AreEqual(EnSmsOperation.AlreadyProcessed, ex.SmsOperation, "Invalid exception type");
			}

			try
			{
				sms.Message = "New content";
				Assert.Fail("Expected InvalidSmsOperationException exception");
			}
			catch (InvalidSmsOperationException ex)
			{
				Assert.AreEqual(EnSmsOperation.AlreadyProcessed, ex.SmsOperation, "Invalid exception type");
			}

			try
			{
				smsc.SendMultipleImmediate(sms);
				Assert.Fail("Expected InvalidSmsOperationException exception");
			}
			catch (InvalidSmsOperationException ex)
			{
				Assert.AreEqual(EnSmsOperation.AlreadyProcessed, ex.SmsOperation, "Invalid exception type");
			}
		}

		[Test]
		public void VerifySmsTransactionNumber_Single()
		{
			ISmsc smsc;
			ISms sms;

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Test sms";

			smsc.SendImmediate(sms);

			Assert.AreNotEqual(Guid.Empty, sms.ReferenceId, "Invalid reference number");
		}

		[Test]
		public void VerifySmsTransactionNumber_Multiple()
		{
			ISmsc smsc;
			ISms sms;
			ISmsStatusCollection status;

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Message = "Test sms";

			status = smsc.SendMultipleImmediate(sms);

			// Verify that the reference numbers are not 0 and are the same for all messages
			Assert.AreNotEqual(Guid.Empty, sms.ReferenceId, "Invalid reference number");
			Assert.IsFalse(string.IsNullOrEmpty(status[0].ProviderReference), "Invalid provider reference");
			Assert.IsFalse(string.IsNullOrEmpty(status[1].ProviderReference), "Invalid provider reference");
		}

		/// <summary>
		/// Send a message to 2 recipients.
		/// If both are successful, the status collection must report
		/// the entire transaction as successful, the error
		/// code and error message should not be set, and
		/// each sms status should be positive with no error
		/// code and no error message
		/// </summary>
		[Test]
		public void SuccessfulMultipleSends()
		{
			ISmsc smsc;
			ISms sms;
			ISmsProvider provider;
			ISmsStatusCollection status;

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			provider = smsc.SmsProviders.Lookup("Fake");

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Message = "Test sms";
			sms.Provider = provider;

			status = smsc.SendMultipleImmediate(sms);

			Assert.IsTrue(status.Successful);
			Assert.AreEqual(0, status.ErrorCode);
			Assert.IsTrue(string.IsNullOrEmpty(status.ErrorMessage));

			foreach (ISmsStatus smsStatus in status)
			{
				Assert.AreEqual(EnMtStatus.Accepted, smsStatus.Status);
				Assert.AreEqual(0, smsStatus.ErrorCode);
				Assert.IsTrue(string.IsNullOrEmpty(smsStatus.ErrorMessage));
			}
		}

		/// <summary>
		/// Send a message to 2 recipients.
		/// If at least one failsthe status collection must report
		/// the entire transaction not successful.
		/// Having the Successful flag set to false
		/// doesn't mean that no sms has been accepted
		/// by the gateway - it simply states that at least one, 
		/// but not all, SMS has not been sent.
		/// 
		/// </summary>
		[Test]
		public void MultipleSends_OneFailure()
		{
			ISmsc smsc;
			ISms sms;
			ISmsProvider provider;
			ISmsStatusCollection status;

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			provider = smsc.SmsProviders.Lookup("Fake");

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Message = "Test sms";
			sms.Provider = provider;
			sms.CustomProperties["0"] = "Custom error";

			status = smsc.SendMultipleImmediate(sms);

			Assert.IsFalse(status.Successful);
			Assert.AreEqual(0, status.ErrorCode);
			Assert.IsTrue(string.IsNullOrEmpty(status.ErrorMessage));

			Assert.AreEqual(EnMtStatus.Failure, status[0].Status);
			Assert.AreNotEqual(0, status[0].ErrorCode);
			Assert.IsFalse(string.IsNullOrEmpty(status[0].ErrorMessage));

			Assert.AreEqual(EnMtStatus.Accepted, status[1].Status);
			Assert.AreEqual(0, status[1].ErrorCode);
			Assert.IsTrue(string.IsNullOrEmpty(status[1].ErrorMessage));

		}
	}
}