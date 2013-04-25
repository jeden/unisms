using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;

namespace Elapsus.Usms.Gateway.Envelope
{
	internal class Sms : ISms
	{
		#region Private Data Members

		private static readonly Regex _regexSender = new Regex(@"^[a-zA-Z0-9\!\@]{1,11}$", RegexOptions.Compiled);
		private static readonly Regex _regexMobileNumber = new Regex(@"^[1-9](\d{8,23})$", RegexOptions.Compiled);

		private readonly List<string> _recipients = new List<string>();
		private readonly List<RecipientFormatError> _recipientErrors = new List<RecipientFormatError>();
		private bool _processed;
		private string _message;
		private Dictionary<string, object> _customProperties;

		#endregion

		#region ISms Members

		public Guid ApplicationId { get; set; }
		public Guid ReferenceId { get; internal set; }
		public ISmsProvider Provider { get; set; }
		public ReadOnlyCollection<string> Recipients { get { return _recipients.AsReadOnly(); } }
		public string Sender { get; set; }
		public string Message
		{
			get { return _message; }
			set { 
				if (_processed)
					throw new InvalidSmsOperationException(EnSmsOperation.AlreadyProcessed, this);

				_message = value; 
			}
		}
		
		public void AddRecipient(string recipient)
		{
			if (_processed)
				throw new InvalidSmsOperationException(EnSmsOperation.AlreadyProcessed, this);

			_recipients.Add(recipient);
		}

		public Dictionary<string, object> CustomProperties { get { if (_customProperties == null) _customProperties = new Dictionary<string, object>(); return _customProperties; } }

		#endregion

		internal bool Processed { get { return _processed; } }

		internal Sms()
		{
			_processed = false;
		}

		internal void SetProcessed()
		{
			_processed = true;
		}

		internal static Sms SafeCast(IReadOnlySms message)
		{
			Sms sms;

			sms = new Sms
			      	{
			      		ApplicationId = message.ApplicationId,
			      		ReferenceId = message.ReferenceId,
			      		Sender = message.Sender,
			      		Message = message.Message
			      	};

			foreach (string recipient in message.Recipients)
				sms.AddRecipient(recipient);

			return sms;
		}

    /// <summary>
    /// Verify that the message is well formed.
    /// Check for inconsistency, missing fields, wrong field formats.
    /// 
    /// On verification failure throws an exception.
    /// </summary>
    /// 
		/// <exception cref="Elapsus.Usms.Gateway.Interface.Exceptions.SmsFormatException">
		/// When the message doesn't have a valid format (missing or invalid field)
		/// </exception>
		/// 
		/// <exception cref="Elapsus.Usms.Gateway.Interface.Exceptions.SmsRecipientFormatException">
		/// When no recipient is found, or one or more recipients either are empty or have invalid format
		/// </exception>
		internal virtual void VerifyMessage()
		{
			if (ApplicationId == Guid.Empty)
				throw new InvalidSmsOperationException(EnSmsOperation.MissingApplicationId, this);

			if (string.IsNullOrEmpty(Message))
				throw new SmsBodyFormatException(this, EnSmsBodyFormat.Missing);
			
			if (string.IsNullOrEmpty(Sender))
				throw new SmsSenderFormatException(this, EnSmsSenderFormat.Missing);
    	VerifySender();

			if (_recipients.Count == 0)
				throw new SmsRecipientFormatException(this);
			VerifyRecipients();
			for (int index = 0; index < Recipients.Count; ++index)
			{
				if (string.IsNullOrEmpty(Recipients[index]))
					_recipientErrors.Add(new RecipientFormatError(Recipients[index], index, EnSmsRecipientFormat.Missing));
			}

			if (_recipientErrors.Count > 0)
				throw new SmsRecipientFormatException(this, _recipientErrors.ToArray());

		}

		private void VerifySender()
		{
			if (Sender.StartsWith("+") || Sender.StartsWith("00"))
			{
				Sender = NormalizeMobileNumber(Sender);
				if(IsValidMobileNumber(Sender) == false)
					throw new SmsSenderFormatException(this, EnSmsSenderFormat.Numeric_InvalidFormat);
			}
			else
			{
				if (_regexSender.IsMatch(Sender) == false)
					throw new SmsSenderFormatException(this, EnSmsSenderFormat.Textual_InvalidFormat);
			}
		}

		private void VerifyRecipients()
		{
			for (int count = 0; count < Recipients.Count; ++count)
				_recipients[count] = NormalizeMobileNumber(_recipients[count]);
		}

		private static bool IsValidMobileNumber(string number)
		{
			return _regexMobileNumber.IsMatch(number);
		}

		private static string NormalizeMobileNumber(string number)
		{
			string num;

			num = (string) number.Clone();
			if (num.StartsWith("+"))
				num = num.Remove(0, 1);
			else if (num.StartsWith("00"))
				num = num.Remove(0, 2);

			return num.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "");
		}

		public override string ToString()
		{
			return string.Format("{0} Recipients - Sender: {1} - Message: {2}", Recipients.Count, Sender,
			                     Message.Replace("\n", " ").Replace("\r", ""));
		}
	}
}
