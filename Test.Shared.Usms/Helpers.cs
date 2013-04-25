using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Envelope;
using NUnit.Framework;

namespace Test.Shared.Usms.Gateway
{
	public class Helpers
	{
		public static void CompareSms(IReadOnlySms sms1, IReadOnlySms sms2)
		{
			Assert.IsNotNull(sms1);
			Assert.IsNotNull(sms2);
			Assert.AreEqual(sms1.ReferenceId, sms2.ReferenceId);
			Assert.AreEqual(sms1.Sender, sms2.Sender);
			Assert.AreEqual(sms1.Recipients.Count, sms2.Recipients.Count);
			for (int count = 0; count < sms1.Recipients.Count; ++count)
				Assert.IsTrue(sms2.Recipients.Contains(sms1.Recipients[count]));
			Assert.AreEqual(sms1.Message, sms2.Message);
		}

		public static ISms FillSms()
		{
			ISms sms;

			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "test message";

			return sms;
		}
	}
}
