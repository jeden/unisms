using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;

namespace Elapsus.Usms.Gateway.Providers
{
	/// <summary>
	/// Base class implementing base features inherited
	/// by all Sms Handlers
	/// </summary>
	public abstract class SmsGateway : Plugin<ISmsProvider>, ISmsGateway
	{
		#region ISmsGateway Members

		public bool IsConnected { get; protected set; }
		public bool KeepConnectionOpen { get; protected set; }
		public bool EmulationMode { get; protected set; }

		public abstract void Connect();
		public abstract void Disconnect();
		protected abstract ISmsStatusCollection OnSend(ISms sms);

		protected SmsGateway(ISmsProvider smsProvider) : base (smsProvider)
		{
		}

		public ISmsStatusCollection Send(ISms sms)
		{
			if (IsConnected == false)
				Connect();

			return OnSend(sms);
		}

		#endregion
	}
}