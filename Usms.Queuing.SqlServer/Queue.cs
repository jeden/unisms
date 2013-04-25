using Elapsus.Usms.Data.SqlServer;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers.Queue;

namespace Elapsus.Usms.Queuing.SqlServer
{
	public class Queue : QueueHandler
	{
		public Queue(IQueueProvider provider) : base(provider)
		{
		}

		public override void Enqueue(ISms sms)
		{
			using (var db = new SmsLogDataContext())
			{
				db.SmsQueue_Enqueue(sms);
			}
		}

		public override IDequeuer GetDequeuer()
		{
			return new Dequeuer();
		}

		public override bool IsEmpty()
		{
			int? queueSize;

			using (var db = new SmsLogDataContext())
			{
				queueSize = db.SmsQueue_GetQueueSize();
			}

			return ((queueSize != null) && (queueSize == 0));
		}

		public override void Initialize()
		{
		}

		public override void VerifyConfiguration()
		{
		}
	}
}
