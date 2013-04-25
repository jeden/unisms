using System;

namespace Elapsus.Usms.Gateway.Notification.Clickatell
{
	internal enum NotificationCode
	{
		MessageUnknown = 0x001,
		MessageQueued = 0x002,
		DeliveredToGateway = 0x003,
		ReceivedByRecipient = 0x004,
		ErrorWithMessage = 0x005,
		UserCancelledMessageDelivery = 0x006,
    ErrorDeliveringMessage = 0x007,
		OK = 0x008,
		RoutingError = 0x009,
		MessageExpired = 0x00A,
		MessageQueuedForLaterDelivery = 0x00B,
		OutOfCredit = 0x00C
	}

	public class ClickatellNotifier : Notifier
	{
		public override void ProcessNotification(DateTime notifiedOn, string providerReference, string recipient, int notificationCode, string notificationMessage)
		{
			NotificationCode code;
			bool isDeliveryError;
			bool isDelivered;

			code = (NotificationCode) notificationCode;

			switch(code)
			{
				case NotificationCode.OK:
				case NotificationCode.MessageQueuedForLaterDelivery:
				case NotificationCode.MessageUnknown:
				case NotificationCode.MessageQueued:
					isDelivered = false;
					isDeliveryError = false;
					break;

				case NotificationCode.ReceivedByRecipient:
				case NotificationCode.DeliveredToGateway:
					isDeliveryError = false;
					isDelivered = true;
					break;

				case NotificationCode.ErrorWithMessage:
				case NotificationCode.UserCancelledMessageDelivery:
				case NotificationCode.ErrorDeliveringMessage:
				case NotificationCode.RoutingError:
				case NotificationCode.MessageExpired:
				case NotificationCode.OutOfCredit:
				default:
					isDelivered = false;
					isDeliveryError = true;
					break;
			}

			if (isDeliveryError)
				NotifyDeliveryFailure(notifiedOn, providerReference, recipient, notificationCode, notificationMessage);
			else if (isDelivered)
				NotifyDelivery(notifiedOn, providerReference, recipient, notificationCode);
			else
				NotifyDeliveryStatusUpdate(notifiedOn, providerReference, recipient, notificationCode, notificationMessage);
		}
	}
}