namespace Elapsus.Usms.Gateway.Interface.Providers
{
	public interface IPlugin
	{
		void VerifyConfiguration();
		void Initialize();
	}
}