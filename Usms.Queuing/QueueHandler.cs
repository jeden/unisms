using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers;
using Elapsus.Usms.Gateway.Interface.Providers.Queue;

namespace Elapsus.Usms.Queuing
{
	public abstract class QueueHandler : Plugin<IQueueProvider>, IQueueHandler
	{
		protected QueueHandler(IQueueProvider provider) : base (provider)
		{
		}

		#region IQueueHandler Members

		public abstract void Enqueue(ISms sms);
		public abstract IDequeuer GetDequeuer();
		public abstract bool IsEmpty();

		#endregion
	}
}
