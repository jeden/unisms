using System;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;

namespace Elapsus.Usms.Gateway.Interface.Exceptions
{
	public sealed class InvalidSmsOperationException : Exception
	{
		private readonly EnSmsOperation _enSmsOperation;

		public EnSmsOperation SmsOperation { get { return _enSmsOperation; } }

		public InvalidSmsOperationException(EnSmsOperation smsOperation, ISms sms) :
			base(string.Format("Invalid operation - Reason: {0}", smsOperation.ToString()))
		{
			_enSmsOperation = smsOperation;
		}
	}
}
