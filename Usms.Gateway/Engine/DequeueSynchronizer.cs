using System.Threading;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers.Queue;

namespace Elapsus.Usms.Gateway.Engine
{
	internal class DequeueSynchronizer
	{
		private class WorkerParams
		{
			public int ThreadCounter { get; private set; }
			public IDequeuer Dequeuer { get; private set; }
			public WorkerParams(int threadCounter, IDequeuer dequeuer)
			{
				ThreadCounter = threadCounter;
				Dequeuer = dequeuer;
			}
		}

		private readonly Thread[] _workers;
		private readonly ForwarderDelegate _forwarder;
		private long _runningWorkers;
		private bool _terminating;
		private bool _suspending = true;
		private readonly EventWaitHandle _suspendWorkersEvent;
		private readonly EventWaitHandle[] _runningWorkerEvents;
		private readonly EventWaitHandle[] _suspendedWorkerEvents;
		private readonly EventWaitHandle[] _terminatingWorkerEvents;

		public int TotalWorkers { get; private set; }
		public int RunningWorkers { get { return (int) Interlocked.Read(ref _runningWorkers); } }

		public DequeueSynchronizer(int workers, IQueueHandler queueHandler, ForwarderDelegate forwarder)
		{
			TotalWorkers = workers;
			_runningWorkers = workers;
			_workers = new Thread[workers];
			_forwarder = forwarder;

			_suspendWorkersEvent = new EventWaitHandle(true, EventResetMode.ManualReset);
			_runningWorkerEvents = new EventWaitHandle[workers];
			_suspendedWorkerEvents = new EventWaitHandle[workers];
			_terminatingWorkerEvents = new EventWaitHandle[workers];

			for (int count = 0; count < workers; ++count)
			{
				_runningWorkerEvents[count] = new EventWaitHandle(false, EventResetMode.ManualReset);
				_suspendedWorkerEvents[count] = new EventWaitHandle(false, EventResetMode.ManualReset);
				_terminatingWorkerEvents[count] = new EventWaitHandle(false, EventResetMode.ManualReset);
				_workers[count] = new Thread(DequeueWorker);
				_workers[count].Start(new WorkerParams(count, queueHandler.GetDequeuer()));
			}

			SuspendWorkers();
		}

		public void ResumeWorkers()
		{
			_suspendWorkersEvent.Set();
			WaitHandle.WaitAll(_runningWorkerEvents);
		}

		public void SuspendWorkers()
		{
			_suspending = true;
			_suspendWorkersEvent.Reset();
			WaitHandle.WaitAll(_suspendedWorkerEvents);
			_suspending = false;
		}

		public void TerminateWorkers()
		{
			_terminating = true;

			if (RunningWorkers < TotalWorkers)
				ResumeWorkers();

			WaitHandle.WaitAll(_terminatingWorkerEvents);
		}

		private void CheckSuspension(int threadCounter)
		{
			if (_suspending)
			{
				Interlocked.Decrement(ref _runningWorkers);

				_runningWorkerEvents[threadCounter].Reset();
				_suspendedWorkerEvents[threadCounter].Set();

				_suspendWorkersEvent.WaitOne();

				Interlocked.Increment(ref _runningWorkers);

				_suspendedWorkerEvents[threadCounter].Reset();
				_runningWorkerEvents[threadCounter].Set();
			}
		}

		private void DequeueWorker(object param)
		{
			WorkerParams workerParams;
			IReadOnlySms sms;

			workerParams = (WorkerParams) param;

			do
			{
				CheckSuspension(workerParams.ThreadCounter);        

				sms = workerParams.Dequeuer.Dequeue();
				if (sms != null)
				{
					_forwarder(sms);
					workerParams.Dequeuer.DequeueCommit(true);
				}
				else
					Thread.Sleep(1000);
				
			} while (_terminating == false);

			Interlocked.Decrement(ref _runningWorkers);
			_terminatingWorkerEvents[workerParams.ThreadCounter].Set();
		}

		/*
		private void DequeueWorker(object param)
		{
			IDequeuer dequeuer;
			IReadOnlySms sms;

			dequeuer = (IDequeuer) param;

			while (syncParams.Terminate == false)
			{
				syncParams.Suspended();
				syncParams.WaitForStartEvent();
				syncParams.Resumed();

				sms = dequeuer.Dequeue();
				if (sms != null)
				{
					_forwarder(sms);
					dequeuer.DequeueCommit(true);
				}
				else
					Thread.Sleep(1000);
			}
		}
		*/
		/*
		private readonly EventWaitHandle _startEvent;
		private readonly EventWaitHandle _suspendedEvent;
		private int _running;

		public bool Terminate { get; set; }

		public void Suspended()
		{
			Interlocked.Decrement(ref _running);
			if (_running == 0)
				_suspendedEvent.Set();
		}

		public void Resumed()
		{
			if (_running == 0)
				_suspendedEvent.Reset();

			Interlocked.Increment(ref _running);
		}

		public void WaitForThreadsSuspended()
		{
			_suspendedEvent.WaitOne();
		}

		public void WaitForStartEvent()
		{
			_startEvent.WaitOne();
		}

		public void SuspendThreads()
		{
			_startEvent.Reset();
		}

		public void ResumeThreads()
		{
			_startEvent.Set();
		}

		internal DequeueSynchronizer(int threads)
		{
			_startEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
			_suspendedEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
			_running = threads;
			Terminate = false;
		}
		 */
	}
}
