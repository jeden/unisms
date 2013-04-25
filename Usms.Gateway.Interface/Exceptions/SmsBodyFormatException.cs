using Elapsus.Usms.Gateway.Interface.Envelope;

namespace Elapsus.Usms.Gateway.Interface.Exceptions
{
	public sealed class SmsBodyFormatException : SmsFormatException
	{
		private readonly EnSmsBodyFormat _smsBodyFormat;

		public EnSmsBodyFormat BodyFormatError { get { return _smsBodyFormat; } }

		public SmsBodyFormatException(ISms sms, EnSmsBodyFormat bodyFormatError) :
			base(sms, EnSmsFormat.InvalidBody, string.Format("Invalid body"))
		{
			_smsBodyFormat = bodyFormatError;
		}
	}
}
