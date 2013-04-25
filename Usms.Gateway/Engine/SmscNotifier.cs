using Elapsus.Usms.Gateway.Interface.Engine;

namespace Elapsus.Usms.Gateway.Engine
{
	internal class SmscNotifier : BaseSmsc, INotificationService
	{
		#region INotificationService Members

		public void DeliveryNotification(NotificationArgs args)
		{
			NotifyDelivery(args);
		}

		public void StatusNotification(NotificationArgs args)
		{
			NotifyDeliveryStatusUpdate(args);
		}

		public void ErrorNotification(NotificationArgs args)
		{
			NotifyDeliveryFailure(args);
		}

		#endregion
	}
}
