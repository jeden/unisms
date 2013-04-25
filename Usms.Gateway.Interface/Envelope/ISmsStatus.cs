using System.Collections.Generic;

namespace Elapsus.Usms.Gateway.Interface.Envelope
{
	public interface ISmsStatus
	{
		bool TestMode { get; }
		EnMtStatus Status { get; }
		int ErrorCode { get; }
		string ErrorMessage { get; }
		string Recipient { get; }
		string ProviderReference { get; }
	}

	public interface ISmsStatusCollection : IList<ISmsStatus>
	{
		ISms Sms { get; }
		bool Successful { get; }
		int ErrorCode { get; }
		string ErrorMessage { get; }
	}
}