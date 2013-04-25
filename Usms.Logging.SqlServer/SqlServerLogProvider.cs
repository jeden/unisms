using System;
using Elapsus.Usms.Data.SqlServer;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers.Log;

namespace Elapsus.Usms.Logging.SqlServer
{
	public enum EnRecipientStatus
	{
		DeliveryFailure = -2,
		AcceptanceFailure = -1,
		Inserted = 0,
		Enqueued = 1,
		Submitted = 2,
		Delivered = 3
	}
	public class SqlServerLogProvider : LogHandler
	{
		public SqlServerLogProvider(ILogProvider provider) : base(provider)
		{
		}

		/// <summary>
		/// Initialize the plugin
		/// </summary>
		public override void Initialize()
		{
		}

		/// <summary>
		/// Verify that all configuration parameters have been
		/// provided in the configuration file
		/// </summary>
		public override void VerifyConfiguration()
		{
		}

		public override void OnAcceptance(ISms sms, DateTime timeStamp)
		{
			using (var db = new SmsLogDataContext())
			{
				db.SmsLog_Insert(sms, timeStamp);
				db.SubmitChanges();
			}
		}

		public override void OnEnqueuement(ISms sms, DateTime timeStamp)
		{
		}

		public override void OnSubmission(ISms sms, ISmsStatusCollection status, DateTime timeStamp)
		{
			SmsLogDataContext.SmsLog_Submitted(sms, status, timeStamp);
		}

		public override void OnSubmissionFailure(ISms sms, ISmsStatusCollection status, DateTime timeStamp)
		{
			SmsLogDataContext.SmsLog_Submitted(sms, status, timeStamp);
		}

		public override void OnDelivery(NotificationArgs args)
		{
			SmsLogDataContext.DeliveryConfirmation(args.ProviderReference, args.Recipient, args.NotifiedOn, args.NotificationCode);
		}

		public override void OnDeliveryFailure(NotificationArgs args)
		{
			SmsLogDataContext.DeliveryFailure(args.ProviderReference, args.Recipient, args.NotifiedOn, args.NotificationCode, args.NotificationMessage);
		}

		public override void OnDeliveryStatusUpdate(NotificationArgs args)
		{
			SmsLogDataContext.DeliveryStatusUpdate(args.ProviderReference, args.Recipient, args.NotifiedOn, args.NotificationCode, args.NotificationMessage);
		}
	}
}
