using System.Data.Linq;

namespace Elapsus.Usms.Data.SqlServer
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Collections.ObjectModel;
	using Gateway.Interface.Envelope;

	partial class SmsLog : IReadOnlySms
	{
		#region IReadOnlySms Members


		public Guid ReferenceId { get { return SmsLogId; } }
		public ReadOnlyCollection<string> Recipients
		{
			get
			{
				string[] recipients;
				recipients = new string[SmsLogRecipients.Count];
				for (int count = 0; count < SmsLogRecipients.Count; ++count)
					recipients[count] = SmsLogRecipients[count].Recipient;
				return new ReadOnlyCollection<string>(recipients);
			}
		}

		#endregion
	}

	partial class SmsLogDataContext
	{
		public void SmsQueue_Enqueue(ISms sms)
		{
			ISingleResult<SmsQueue_EnqueueResult> ret;
			SmsQueue_EnqueueResult res;
			ret = SmsQueue_Enqueue(sms.ReferenceId, sms.ApplicationId, sms.Sender, sms.Recipients.Count, sms.Message, DateTime.Now, ConvertRecipients(sms.Recipients));

			res = ret.ReturnValue as SmsQueue_EnqueueResult;

		}

		public void SmsLog_Insert(ISms sms, DateTime timeStamp)
		{
			SmsLog_Insert(sms.ReferenceId, sms.ApplicationId, sms.Sender, sms.Recipients.Count, sms.Message, timeStamp, ConvertRecipients(sms.Recipients), false);
		}

		public static void SmsLog_Submitted(ISms sms, ISmsStatusCollection status, DateTime timeStamp)
		{
			using (var db = new SmsLogDataContext())
			{
				for (int count = 0; count < status.Count; ++count)
					db.SmsLog_Forwarded(sms.ReferenceId, status[count].ProviderReference, timeStamp, sms.Recipients[count], status[count].Status < 0, status[count].ErrorCode, status[count].ErrorMessage);

				db.SubmitChanges();
			}
		}

		public static void DeliveryConfirmation(string providerReference, string recipient, DateTime deliveredOn, int notificationCode)
		{
			using (var db = new SmsLogDataContext())
			{
				db.SmsLog_DeliveryConfirmation(providerReference, recipient, deliveredOn, notificationCode);
				db.SubmitChanges();
			}
		}

		public static void DeliveryStatusUpdate(string providerReference, string recipient, DateTime notifiedOn, int notificationCode, string notificationMessage)
		{
			using (var db = new SmsLogDataContext())
			{
				db.SmsLog_DeliveryStatusUpdate(providerReference, recipient, notifiedOn, notificationCode, notificationMessage);
				db.SubmitChanges();
			}
		}

		public static void DeliveryFailure(string providerReference, string recipient, DateTime failedOn, int errorCode, string errorMessage)
		{
			using (var db = new SmsLogDataContext())
			{
				db.SmsLog_DeliveryFailureNotification(providerReference, recipient, failedOn, errorCode, errorMessage);
				db.SubmitChanges();
			}
		}

		/*
		private SmsLogRecipient FindSmsLogRecipient(string providerReference, string recipient)
		{
			SmsLogRecipient smsLogRecipient = null;

			var s = from log in SmsLogRecipients
			        where
			        	(log.ProviderReference == providerReference) &&
			        	(log.Recipient == recipient)
			        select log;

			if (s.Count() > 0)
				smsLogRecipient = s.Single();

			return smsLogRecipient;
		}
		*/

		private static string ConvertRecipients(IEnumerable<string> recipients)
		{
			StringBuilder recips;

			recips = new StringBuilder();
			foreach (string recipient in recipients)
				recips.AppendFormat("{0},", recipient);
			recips.Remove(recips.Length - 1, 1);

			return recips.ToString();
		}
	}
}
