using System;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers;
using Elapsus.Usms.Gateway.Interface.Providers.Log;

namespace Elapsus.Usms.Logging
{
	public abstract class LogHandler : Plugin<ILogProvider>, ILogHandler
	{
		public abstract void OnAcceptance(ISms sms, DateTime timeStamp);
		public abstract void OnEnqueuement(ISms sms, DateTime timeStamp);
		public abstract void OnSubmission(ISms sms, ISmsStatusCollection status, DateTime timeStamp);
		public abstract void OnSubmissionFailure(ISms sms, ISmsStatusCollection status, DateTime timeStamp);
		public abstract void OnDelivery(NotificationArgs args);
		public abstract void OnDeliveryFailure(NotificationArgs args);
		public abstract void OnDeliveryStatusUpdate(NotificationArgs args);

		protected LogHandler(ILogProvider provider) : base (provider)
		{
		}
	}
}
