using Elapsus.Usms.Gateway.Interface.Envelope;

namespace Elapsus.Usms.Gateway.Interface.Exceptions
{
	public sealed class SmsSenderFormatException : SmsFormatException
	{
		private readonly EnSmsSenderFormat _smsSenderFormat;

		public EnSmsSenderFormat SenderFormatError { get { return _smsSenderFormat; } }

		public SmsSenderFormatException(ISms sms, EnSmsSenderFormat senderFormatError) : 
			base (sms, EnSmsFormat.InvalidSender, string.Format("Invalid sender"))
		{
			_smsSenderFormat = senderFormatError;
		}
	}
}
