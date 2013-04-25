using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers;

namespace Elapsus.Usms.Gateway.Providers
{
	public interface ISmsGateway : IPlugin
	{
		bool IsConnected { get; }
		bool KeepConnectionOpen { get; }
		bool EmulationMode { get; }

		void Connect();
		void Disconnect();
		ISmsStatusCollection Send(ISms sms);
	}
}
