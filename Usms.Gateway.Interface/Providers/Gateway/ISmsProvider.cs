namespace Elapsus.Usms.Gateway.Interface.Providers.Gateway
{
	public interface ISmsProvider : IProvider
	{
		bool TestMode { get; set; }
		ISmsProviderCapabilities Capabilities { get; }
	}
}