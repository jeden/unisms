using System;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;

namespace Elapsus.Usms.Gateway.Notification
{
	public abstract class Notifier
	{
		private readonly INotificationService _notificationService;

		protected Notifier()
		{
			_notificationService = SmsFactory.CreateNotificationService();
		}

		protected virtual void NotifyDelivery(DateTime notifiedOn, string providerReference, string recipient, int notificationCode)
		{
			NotificationArgs args;
      args = NotificationArgs.Delivery(notifiedOn, providerReference, recipient, notificationCode);
			_notificationService.DeliveryNotification(args);
		}

		protected virtual void NotifyDeliveryStatusUpdate(DateTime notifiedOn, string providerReference, string recipient, int notificationCode, string notificationMessage)
		{
			NotificationArgs args;
			args = NotificationArgs.StatusUpdate(notifiedOn, providerReference, recipient, notificationCode, notificationMessage);
			_notificationService.StatusNotification(args);
		}

		protected virtual void NotifyDeliveryFailure(DateTime notifiedOn, string providerReference, string recipient, int errorCode, string errorMessage)
		{
			NotificationArgs args;
			args = NotificationArgs.Failure(notifiedOn, providerReference, recipient, errorCode, errorMessage);
			_notificationService.ErrorNotification(args);
		}

		public abstract void ProcessNotification(DateTime notifiedOn, string providerReference, string recipient, int notificationCode, string notificationMessage);
	}
}
