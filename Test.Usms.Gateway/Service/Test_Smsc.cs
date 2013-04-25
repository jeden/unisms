using System;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using NUnit.Framework;
using Test.Shared.Usms.Gateway;

namespace Test.Usms.Gateway.Service
{
	[TestFixture]
	public class Test_Smsc
	{
		/// <summary>
		/// Verify that if an application id is not provided to
		/// the ISms, an InvalidSmsOperationException exception
		/// is thrown
		/// </summary>
		[Test]
		public void MissingApplicationId()
		{
			ISms sms;
			ISmsc smsc;

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(Guid.Empty);

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Test message";

			try
			{
				smsc.SendImmediate(sms);
				Assert.Fail("Expected InvalidSmsOperationException");
			}
			catch (InvalidSmsOperationException ex)
			{
				if (ex.SmsOperation != EnSmsOperation.MissingApplicationId)
					Assert.Fail("Unexpected Sms Operation reason");
			}
		}

		[Test]
		public void CreationOfReferenceNumber()
		{
			ISms sms;

			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			Assert.AreNotEqual(Guid.Empty, sms.ReferenceId);
		}
	}
}