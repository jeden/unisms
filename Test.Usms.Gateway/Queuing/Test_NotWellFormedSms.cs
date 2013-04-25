using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using NUnit.Framework;
using Test.Shared.Usms.Gateway;
using TypeMock;

namespace Test.Usms.Gateway.Queuing
{
	[TestFixture]
	public class Test_NotWellFormedSms
	{
		[SetUp]
		public void SetUp()
		{
			MockManager.Init();
			MockService.MockQueueProvider();
		}

		/// <summary>
		/// Attempt to send a not well formed sms, having
		/// a missing field
		/// 
		/// In this test, the recipient is left empty
		/// </summary>
		[Test]
		public void Test_MissingRecipient()
		{
			ISms sms;
			ISmsc smsc;

			try
			{
				smsc = SmsFactory.GetSmsc();

				sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
				sms.Sender = TestConstants.Default_Sender;
				sms.Message = "This is a test";

				smsc.Enqueue(sms);

				Assert.Fail("Expected SmsFormatException");
			}
			catch (SmsRecipientFormatException ex)
			{
				Assert.AreEqual(EnSmsFormat.InvalidRecipient, ex.SmsFormat, "Expected Invalid Recipient reason");
				Assert.IsEmpty(ex.RecipientFormatErrors, "Expected Missing Recipient reason");
			}
		}

		/// <summary>
		/// Add an empty recipient
		/// </summary>
		[Test]
		public void Test_EmptyRecipient()
		{
			ISms sms;
			ISmsc smsc;

			try
			{
				smsc = SmsFactory.GetSmsc();

				sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
				sms.Sender = TestConstants.Default_Sender;
				sms.Message = "This is a test";
				sms.AddRecipient("");

				smsc.Enqueue(sms);

				Assert.Fail("Expected SmsRecipientFormatException");
			}
			catch (SmsRecipientFormatException ex)
			{
				Assert.AreEqual(EnSmsFormat.InvalidRecipient, ex.SmsFormat, "Expected Invalid Recipient reason");
				Assert.IsNotEmpty(ex.RecipientFormatErrors, "No recipient error reported");
				Assert.AreEqual(1, ex.RecipientFormatErrors.Length, "Invalid number of recipient errors");
				Assert.AreEqual(0, ex.RecipientFormatErrors[0].Index);
				Assert.AreEqual(string.Empty, ex.RecipientFormatErrors[0].Recipient);
				Assert.AreEqual(EnSmsRecipientFormat.Missing, ex.RecipientFormatErrors[0].RecipientFormat, "Invalid recipient reason");
			}
		}

		/// <summary>
		/// Sender is empty
		/// </summary>
		[Test]
		public void Test_MissingSender()
		{
			ISms sms;
			ISmsc smsc;

			try
			{
				smsc = SmsFactory.GetSmsc();

				sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
				sms.AddRecipient(TestConstants.Default_Recipient);
				sms.Message = "this is a test";

				smsc.Enqueue(sms);

				Assert.Fail("Expected SmsFormatException");
			}
			catch (SmsSenderFormatException ex)
			{
				Assert.AreEqual(EnSmsFormat.InvalidSender, ex.SmsFormat, "Expected Invalid Sender reason");
				Assert.AreEqual(EnSmsSenderFormat.Missing, ex.SenderFormatError, "Expected Missing Sender reason");
			}
		}

		/// <summary>
		/// Message body is empty
		/// </summary>
		[Test]
		public void Test_MissingBody()
		{
			ISms sms;
			ISmsc smsc;

			try
			{
				smsc = SmsFactory.GetSmsc();

				sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
				sms.AddRecipient(TestConstants.Default_Recipient);
				sms.Sender = TestConstants.Default_Sender;

				smsc.Enqueue(sms);

				Assert.Fail("Expected SmsFormatException");
			}
			catch (SmsBodyFormatException ex)
			{
				Assert.AreEqual(EnSmsFormat.InvalidBody, ex.SmsFormat, "Expected Invalid Body reason");
				Assert.AreEqual(EnSmsBodyFormat.Missing, ex.BodyFormatError, "Expected Missing Body reason");
			}
		}

		[Test]
		public void Test_InvalidSender_Textual_TooLong()
		{
			ISms sms;
			ISmsc smsc;

			try
			{
				smsc = SmsFactory.GetSmsc();

				sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

				sms.AddRecipient(TestConstants.Default_Recipient);
				sms.Message = "Test SMS";
				sms.Sender = "Sender in more than 11 characters";

				smsc.Enqueue(sms);

				Assert.Fail("Expected SmsSenderFormatException exception");
			}
			catch (SmsSenderFormatException ex)
			{
				Assert.AreEqual(EnSmsSenderFormat.Textual_InvalidFormat, ex.SenderFormatError, "Expected Too Long reason");
			}
		}

		[Test]
		public void NormalizeMobileNumber()
		{
			ISms sms;
			ISmsc smsc;

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			sms.Message = "TestMessage";
			sms.Sender = "+1 (234)567-890.14";
			sms.AddRecipient("00123-456.78 90 (12)");

			smsc.Enqueue(sms);

			Assert.AreEqual("123456789014", sms.Sender, "Sender not normalized");
			Assert.AreEqual("123456789012", sms.Recipients[0], "Recipient not normalized");
		}

		[Test]
		public void VerifyNumberFormats()
		{
			ISms sms;
			ISmsc smsc;

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			try
			{
				sms.Message = "TestMessage";
				sms.Sender = "+48335a23";
				sms.AddRecipient(TestConstants.Default_Recipient);
				smsc.Enqueue(sms);

				Assert.Fail("Expected SmsSenderFormatException");
			}
			catch (SmsSenderFormatException ex)
			{
				Assert.AreEqual(EnSmsSenderFormat.Numeric_InvalidFormat, ex.SenderFormatError, "Expected Invalid Numeric Format error");
			}
		}
	}
}
