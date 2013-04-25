using System;

namespace Elapsus.Usms.Gateway.Interface.Engine
{
	public interface INotificationService
	{
		void DeliveryNotification(NotificationArgs args);
		void StatusNotification(NotificationArgs args);
		void ErrorNotification(NotificationArgs args);
	}

	public class NotificationArgs : EventArgs
	{
		public string ProviderReference { get; private set; }
		public string Recipient { get; private set; }
		public int NotificationCode { get; private set; }
		public string NotificationMessage { get; private set; }
		public bool Successful { get; private set; }
		public bool IsFinalStatus { get; private set; }
		public DateTime NotifiedOn { get; private set; }

		public static NotificationArgs Delivery(DateTime notifiedOn, string providerReference, string recipient, int notificationCode)
		{
			return new NotificationArgs
			       	{
			       		ProviderReference = providerReference,
			       		Recipient = recipient,
			       		NotifiedOn = notifiedOn,
			       		Successful = true,
								IsFinalStatus = true,
								NotificationCode = notificationCode
			       	};
		}

		public static NotificationArgs StatusUpdate(DateTime notifiedOn, string providerReference, string recipient, int notificationCode, string notificationMessage)
		{
			return new NotificationArgs
			       	{
			       		ProviderReference = providerReference,
			       		Recipient = recipient,
			       		NotifiedOn = notifiedOn,
			       		Successful = false,
								IsFinalStatus = false,
			       		NotificationCode = notificationCode,
			       		NotificationMessage = notificationMessage
			       	};
		}
		
		public static NotificationArgs Failure(DateTime notifiedOn, string providerReference, string recipient, int errorCode, string errorMessage)
		{
			return new NotificationArgs
			       	{
			       		ProviderReference = providerReference,
			       		Recipient = recipient,
			       		NotifiedOn = notifiedOn,
			       		Successful = false,
			       		IsFinalStatus = true,
			       		NotificationCode = errorCode,
			       		NotificationMessage = errorMessage
			       	};
		}
	}
}