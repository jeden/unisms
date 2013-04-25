using System;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;
using Elapsus.Usms.Gateway.Interface.Providers.Log;
using Elapsus.Usms.Gateway.Interface.Providers.Queue;

namespace Elapsus.Usms.Gateway.Interface.Engine
{
	public enum EnSmsOperation
	{
		MultipleRecipients,
		AlreadyProcessed,
		MissingApplicationId
	}

	public class SmsNotificationArgs : EventArgs
	{
		private readonly ISms _sms;
		private readonly ISmsStatus _smsStatus;

		public ISms Sms { get { return _sms; } }
		public ISmsStatus SmsStatus { get { return _smsStatus; } }

		public SmsNotificationArgs(ISms sms, ISmsStatus smsStatus)
		{
			_sms = sms;
			_smsStatus = smsStatus;
		}
	}

	public delegate void SmsNotificationEventHandler(object sender, SmsNotificationArgs args);

	public interface ISmsc
	{
		IProviders<ISmsProvider> SmsProviders { get; }
		IProviders<ILogProvider> LogProviders { get; }
		IProviders<IQueueProvider> QueueProviders { get; }

		ISmsStatus SendImmediate(ISms message);
		ISmsStatusCollection SendMultipleImmediate(ISms message);

		void Enqueue(ISms sms);
		bool IsQueueEmpty();
		IDequeuer GetDequeuer();
	}
}