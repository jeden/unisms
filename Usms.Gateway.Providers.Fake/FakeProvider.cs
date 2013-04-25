using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;

namespace Elapsus.Usms.Gateway.Providers.Fake
{
	public sealed class FakeProvider : SmsGateway
	{
		public FakeProvider (ISmsProvider provider) : base(provider)
		{
		}

		public override void Connect()
		{
		}

		public override void Disconnect()
		{
		}

		protected override ISmsStatusCollection OnSend(ISms sms)
		{
			SmsStatusCollection status;
			SmsStatus smsStatus;
			int? failCode = null;

			if (sms.CustomProperties.ContainsKey("FailCode"))
				failCode = int.Parse(sms.CustomProperties["FailCode"].ToString()	);

			status = new SmsStatusCollection(sms);
			for (int count = 0; count < sms.Recipients.Count; ++count)
			{
				smsStatus = new SmsStatus(sms.Recipients[count]);
				smsStatus.ProviderReference = sms.ReferenceId.ToString();
				if (failCode != null)
					smsStatus.SetError(EnMtStatus.Failure, (int) failCode, "Error forced for testing purposes");
				else
				{
					if (sms.CustomProperties.ContainsKey(count.ToString()))
						smsStatus.SetError(EnMtStatus.Failure, -1, "Error forced using sms custom properties");
				}

				status.Add(smsStatus);
			}

			return status;
		}

		public override void VerifyConfiguration() {}
		public override void Initialize() {}
	}
}
