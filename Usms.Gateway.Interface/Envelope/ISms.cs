using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;

namespace Elapsus.Usms.Gateway.Interface.Envelope
{
	public enum EnMtStatus {
		Failure = -1,
		Accepted = 1,		// The SMS-C/Gateway has accepted the message - this doesn't mean it has been delivered
		Multiple = 101	// When sending to more than 1 recipients this value indicates that 
		// actual status is stored elsewhere for each instance of the sms
	}

	public enum EnSmsFormat {
		WellFormed = 0,
		InvalidRecipient = -11,
		InvalidSender = -12,
		InvalidBody = -13
	}

	public enum EnSmsRecipientFormat {
		WellFormed = 0,
		Missing = -10
	}

	public enum EnSmsSenderFormat {
		WellFormed = 0,
		Missing = -10,
		Textual_InvalidFormat = -11,
		Numeric_InvalidFormat = -12
	}

	public enum EnSmsBodyFormat
	{
		WellFormed = 0,
		Missing = -10,
	}

	public interface IReadOnlySms
	{
		Guid ApplicationId { get; }
		Guid ReferenceId { get; }
		string Sender { get; set; }
		string Message { get; set; }
		ReadOnlyCollection<string> Recipients { get; }
	}

	public interface ISms : IReadOnlySms
	{
		new Guid ApplicationId { get; }
		new Guid ReferenceId { get; }
		new string Sender { get; set; }
		new string Message { get; set; }
		new ReadOnlyCollection<string> Recipients { get; }
		ISmsProvider Provider { get; set; }

		Dictionary<string, object> CustomProperties { get; }

		void AddRecipient(String recipient);
	}
}