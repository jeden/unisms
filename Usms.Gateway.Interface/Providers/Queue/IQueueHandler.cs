using System;
using Elapsus.Usms.Gateway.Interface.Envelope;

namespace Elapsus.Usms.Gateway.Interface.Providers.Queue
{
	public delegate ISms SmsInstantiatorDelegate();

	public interface IQueueHandler : IPlugin
	{
		void Enqueue(ISms sms);
		IDequeuer GetDequeuer();
		bool IsEmpty();
	}

	public interface IDequeuer : IDisposable
	{
		IReadOnlySms Dequeue();
		void DequeueCommit(bool processed);
	}
}