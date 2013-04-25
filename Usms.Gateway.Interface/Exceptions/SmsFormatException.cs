using System;
using Elapsus.Usms.Gateway.Interface.Envelope;

namespace Elapsus.Usms.Gateway.Interface.Exceptions
{
	public class SmsFormatException : Exception
	{
		private readonly EnSmsFormat _enSmsFormat;
		private readonly ISms _sms;

		public EnSmsFormat SmsFormat { get { return _enSmsFormat; } }
		public ISms Sms { get { return _sms; } }

		protected SmsFormatException(ISms sms, EnSmsFormat enSmsFormat, string message)
			: base(message)
		{
			_enSmsFormat = enSmsFormat;
			_sms = sms;
		}

		public SmsFormatException(ISms sms, EnSmsFormat enSmsFormat) : 
			base(string.Format("Invalid SMS format - Reason: {0}", enSmsFormat))
		{
			_enSmsFormat = enSmsFormat;
			_sms = sms;
		}
	}
}
