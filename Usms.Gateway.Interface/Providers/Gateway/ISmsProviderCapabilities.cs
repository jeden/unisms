namespace Elapsus.Usms.Gateway.Interface.Providers.Gateway
{
	public interface ISmsProviderCapabilities
	{
		bool SimpleSms { get; }
		bool TestMode { get; }
	}
}