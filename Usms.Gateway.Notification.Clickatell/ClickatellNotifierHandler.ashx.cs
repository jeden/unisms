using System;
using System.Web;

namespace Elapsus.Usms.Gateway.Notification.Clickatell
{
	public class ClickatellNotifierHandler : IHttpHandler
	{
		/// <summary>
		/// You will need to configure this handler in the web.config file of your 
		/// web and register it with IIS before being able to use it. For more information
		/// see the following link: http://go.microsoft.com/?linkid=8101007
		/// </summary>
		#region IHttpHandler Members

		public bool IsReusable { get { return true; } }

		public void ProcessRequest(HttpContext context)
		{
			//string apiMsgId, from, charge;
			string cliMsgId, timestamp, to, status;

			//apiMsgId = context.Request["apiMsgId"];
			cliMsgId = context.Request["cliMsgId"];
			timestamp = context.Request["timestamp"];
			to = context.Request["to"];
			//from = context.Request["from"];
			//charge = context.Request["charge"];
			status = context.Request["status"];

			if (!string.IsNullOrEmpty(cliMsgId) && !string.IsNullOrEmpty(timestamp) && !string.IsNullOrEmpty(to) && !string.IsNullOrEmpty(status))
			{
				var statusCode = Convert.ToInt32(status);
				var notifier = new ClickatellNotifier();

				notifier.ProcessNotification(DateTime.Now, cliMsgId, to, statusCode, null);
			}
		}

		#endregion
	}
}
