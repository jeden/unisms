using System.Linq;
using Elapsus.Usms.Data.SqlServer;

namespace Test.Shared.Usms.Gateway
{
	public static class DataHelpers
	{
		public static SmsLog GetSmsLog(string providerReference)
		{
			SmsLogDataContext db;

			db = new SmsLogDataContext();

			var smsLog = from log in db.SmsLogs
									 join recipient in db.SmsLogRecipients on log.SmsLogId equals recipient.SmsLogId
									 where recipient.ProviderReference == providerReference
									 select log;
			
			return smsLog.Single();
		}
	}
}
