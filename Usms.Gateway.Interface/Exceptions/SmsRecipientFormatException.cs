using Elapsus.Usms.Gateway.Interface.Envelope;

namespace Elapsus.Usms.Gateway.Interface.Exceptions
{
	public sealed class RecipientFormatError
	{
		public string Recipient { get; set; }
		public int Index { get; set; }
		public EnSmsRecipientFormat RecipientFormat { get; set; }

		public RecipientFormatError(string recipient, int index, EnSmsRecipientFormat recipientFormat)
		{
			Recipient = recipient;
			Index = index;
			RecipientFormat = recipientFormat;
		}
	}

	public sealed class SmsRecipientFormatException : SmsFormatException
	{
		private readonly RecipientFormatError[] _recipientFormatErrors;

		public RecipientFormatError[] RecipientFormatErrors { get { return _recipientFormatErrors; } }

		public SmsRecipientFormatException(ISms sms) : base(sms, EnSmsFormat.InvalidRecipient, "Invalid SMS: no recipient")
		{
			_recipientFormatErrors = new RecipientFormatError[0];
		}

		public SmsRecipientFormatException(ISms sms, RecipientFormatError[] recipientFormatErrors) :
			base(sms, EnSmsFormat.InvalidRecipient, 
				recipientFormatErrors.Length == 1 ? 
					string.Format("Invalid SMS: 1 empty or invalid recipient") :
					string.Format("Invalid SMS: {0} empty or invalid recipients", recipientFormatErrors.Length))
		{
			_recipientFormatErrors = recipientFormatErrors;
		}
	}
}
