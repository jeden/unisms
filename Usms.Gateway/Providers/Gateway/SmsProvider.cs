using Elapsus.Usms.Gateway.Interface.Providers.Gateway;

namespace Elapsus.Usms.Gateway.Providers.Gateway
{
	internal class SmsProvider : Provider, ISmsProvider
	{
		public SmsProvider()
		{
			Capabilities = new SmsProviderCapabilities();
		}

		#region ISmsProvider Members

		public ISmsProviderCapabilities Capabilities { get; internal set; }
		public bool TestMode { get; set; }

		#endregion
	}
}