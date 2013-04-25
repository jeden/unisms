using Elapsus.Usms.Gateway.Interface.Providers.Gateway;

namespace Elapsus.Usms.Gateway.Providers.Gateway
{
	internal class SmsProviderCapabilities : ISmsProviderCapabilities
	{
		#region ISmsProviderCapabilities Members

		public bool SimpleSms { get; internal set; }
		public bool TestMode { get; internal set; }

		#endregion
	}
}