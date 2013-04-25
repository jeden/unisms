using System;
using System.IO;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using Elapsus.Usms.Gateway.Interface.Providers.Log;

namespace Elapsus.Usms.Logging.Text
{
	public class TextLogger : LogHandler
	{
		private static StreamWriter _logFile;
		private static readonly object _logFileLocker = new object();
		private string _fileName;

		public TextLogger(ILogProvider provider) : base (provider)
		{
		}

		public override void VerifyConfiguration()
		{
			if (ContainsSetting(Provider.Configuration, "fileName") == false)
				throw ProviderConfigurationException.MissingParameter(Provider.Name, "fileName");

			_fileName = Provider.Configuration.Settings["fileName"];
		}

		public override void Initialize()
		{
			lock (_logFileLocker)
			{
				if (_logFile == null)
					_logFile = new StreamWriter(_fileName);
			}
		}

		public override void OnAcceptance(ISms sms, DateTime timeStamp)
		{
			WriteLine(string.Format("Accepted:  {0}", sms));
		}

		public override void OnEnqueuement(ISms sms, DateTime timeStamp)
		{
			WriteLine(string.Format("Enqueued:  {0}", sms));
		}

		public override void OnSubmission(ISms sms, ISmsStatusCollection status, DateTime timeStamp)
		{
			WriteLine(string.Format("Submitted: {0}", sms));
			foreach (ISmsStatus st in status)
				WriteIndented(st.ToString());
		}

		public override void OnSubmissionFailure(ISms sms, ISmsStatusCollection status, DateTime timeStamp)
		{
			int failures = 0;

			WriteLine(string.Format("**Failure: {0}", sms));
			foreach (ISmsStatus st in status)
			{
				WriteIndented(st.ToString());
				if (st.Status < 0)
					failures++;
			}

			WriteIndented(string.Format("Number of failures: {0}", failures));
		}

		public override void OnDelivery(NotificationArgs args)
		{
			WriteLine(string.Format("Delivery Confirmation: {0}", args.Recipient));
			WriteIndented(string.Format("Reference: {0}", args.ProviderReference));
		}

		public override void OnDeliveryFailure(NotificationArgs args)
		{
			WriteLine(string.Format("**Delivery Error: {0}", args.Recipient));
			WriteIndented(string.Format("Reference: {0}", args.ProviderReference));
			WriteIndented(string.Format("Error: {0} - {1}", args.NotificationCode, args.NotificationMessage));
		}

		public override void OnDeliveryStatusUpdate(NotificationArgs args)
		{
			WriteLine(string.Format("Status update: {0}", args.Recipient));
			WriteIndented(string.Format("Reference: {0}", args.ProviderReference));
			WriteIndented(string.Format("Notification: {0} - {1}", args.NotificationCode, args.NotificationMessage));
		}

		private static void WriteLine(string message)
		{
			lock (_logFileLocker)
			{
				_logFile.WriteLine(string.Format("[{0:yyyy.MM.dd hh:mm:ss.fff}] {1}", DateTime.Now, message));
				_logFile.Flush();
			}
		}

		private static void WriteIndented(string message)
		{
			lock (_logFileLocker)
			{
				_logFile.WriteLine(string.Format("                          {0}", message));
				_logFile.Flush();
			}
		}
	}
}
