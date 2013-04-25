using System.Collections.Generic;
using Elapsus.Usms.Gateway.Interface.Envelope;

namespace Elapsus.Usms.Gateway.Providers
{
	public class SmsStatus : ISmsStatus
	{
		public SmsStatus(string recipient)
		{
			Recipient = recipient;
			Status = EnMtStatus.Accepted;
			ErrorMessage = null;
		}

		#region ISmsStatus Members

		public bool TestMode { get; set; }
		public EnMtStatus Status { get; private set; }
		public string ErrorMessage { get; private set; }
		public int ErrorCode { get; private set; }
		public string Recipient { get; private set; }
		public string ProviderReference { get; set; }
		public void SetError(EnMtStatus status, int errorCode, string errorMessage)
		{
			Status = status;
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
		}

		#endregion

		public override string ToString()
		{
			string msg;

			msg = string.Format("Recipient: {0} - Provider Reference: {1}", Recipient, ProviderReference);
			if (Status < 0)
				msg += string.Format(" - Error Code: {0} - Error Message: {1} ", ErrorCode, ErrorMessage);

			return msg;
		}
	}

	public class SmsStatusCollection : List<ISmsStatus>, ISmsStatusCollection
	{
		public SmsStatusCollection(ISms sms)
		{
			Sms = sms;
			Successful = true;
		}

		#region ISmsStatusCollection Members

		public ISms Sms { get; internal set; }
		public bool Successful { get; private set; }
		public string ErrorMessage { get; private set; }
		public int ErrorCode { get; private set; }
		public new void Add(ISmsStatus status)
		{
			base.Add(status);
			if (status.Status < 0)
				Successful = false;
		}

		public void SetError(int errorCode, string errorMessage)
		{
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
			Successful = false;
		}

		#endregion
	}
}