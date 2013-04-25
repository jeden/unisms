using System;
using ASPSMS.NET;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;

namespace Elapsus.Usms.Gateway.Providers.Aspsms
{
	public sealed class AspsmsProvider : SmsGateway
	{
		public AspsmsProvider(ISmsProvider smsProvider) : base(smsProvider)
		{
		}

		public override void Connect()
		{
		}

		public override void Disconnect()
		{
		}

		public override void Initialize() { }

		public override void VerifyConfiguration()
		{
			if (ContainsSetting(Provider.Configuration, "userKey") == false)
				throw ProviderConfigurationException.MissingParameter(Provider.Name, "userKey");

			if (ContainsSetting(Provider.Configuration, "password") == false)
				throw ProviderConfigurationException.MissingParameter(Provider.Name, "password");
		}

		protected override ISmsStatusCollection OnSend(ISms sms)
		{
			ASPSMSNET aspsms;
			SmsStatusCollection status;
			SmsStatus smsStatus;

			aspsms = new ASPSMSNET
			{
				UserKey = Provider.Configuration.Settings["userKey"],
				Password = Provider.Configuration.Settings["password"],
				Originator = sms.Sender,
				MessageData = sms.Message,
			};

			foreach (string recipient in sms.Recipients)
				aspsms.Recipients.Add(recipient, ToInt(sms.ReferenceId));

			if (Provider.TestMode == false)
				aspsms.SendTextSMS();

			status = new SmsStatusCollection(sms);
			if ((aspsms.ErrorCode != 1) && (Provider.TestMode == false))
				status.SetError(aspsms.ErrorCode, aspsms.ErrorDescription);

			foreach (string recipient in sms.Recipients)
			{
				smsStatus = new SmsStatus(recipient) {TestMode = Provider.TestMode};
				if (status.Successful == false)
					smsStatus.SetError(EnMtStatus.Failure, status.ErrorCode, status.ErrorMessage);
				status.Add(smsStatus);
			}

			return status;
		}

		private static int ToInt(Guid guid)
		{
			byte[] bytes;
			int value, temp;

			value = 0;
			bytes = guid.ToByteArray();
			
			for (int count = 0; count < bytes.Length; ++count)
			{
				temp = bytes[count] << (count*4);
				value += temp;
			}

			return value;
		}
	}
}
