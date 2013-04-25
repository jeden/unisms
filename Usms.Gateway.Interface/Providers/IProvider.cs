namespace Elapsus.Usms.Gateway.Interface.Providers
{
	public interface IProvider
	{
		bool Enabled { get; }
		string Name { get; }
		IConfiguration Configuration { get; }
	}
}