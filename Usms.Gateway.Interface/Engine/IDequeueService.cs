namespace Elapsus.Usms.Gateway.Interface.Engine
{
	public interface IDequeueService
	{
		int RunningWorkers { get; }
		int TotalWorkers { get; }
		void Start();
		void Suspend();
		void Terminate();
		bool IsQueueEmpty();
	}
}