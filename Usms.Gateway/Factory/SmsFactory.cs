using System;
using Elapsus.Usms.Gateway.Engine;
using Elapsus.Usms.Gateway.Envelope;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;

namespace Elapsus.Usms.Gateway.Factory
{
	public static class SmsFactory
	{
		public static ISms NewSms(Guid applicationId)
		{
			var sms = new Sms
			          	{
			          		ApplicationId = applicationId,
										ReferenceId = Guid.NewGuid()
			          	};
			return sms;
		}

		public static ISmsc GetSmsc()
		{
			return new Smsc();
		}

		public static IDequeueService CreateDequeueService(int dequeuers)
		{
			return new Smsc(dequeuers);
		}

		public static INotificationService CreateNotificationService()
		{
			return new SmscNotifier();
		}
	}
}
