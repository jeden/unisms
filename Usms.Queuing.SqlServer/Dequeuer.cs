using System;
using System.Data;
using System.Data.Linq;
using System.Linq;
using Elapsus.Usms.Data.SqlServer;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers.Queue;

namespace Elapsus.Usms.Queuing.SqlServer
{
	internal class Dequeuer : IDequeuer
	{
		private SmsLogDataContext _db = new SmsLogDataContext();
		private Guid _transactionId = Guid.Empty;

		public IReadOnlySms Dequeue()
		{
			ISingleResult<SmsLog> smsResult;
			Guid? transId;
			IReadOnlySms sms;

			transId = Guid.Empty;

			if (_db.Connection.State == ConnectionState.Closed)
				_db.Connection.Open();

			_db.Transaction = _db.Connection.BeginTransaction();
			smsResult = _db.SmsQueue_Dequeue(ref transId);
			_transactionId = (Guid)transId;

			switch ((int)smsResult.ReturnValue)
			{
				case 1:
					sms = smsResult.Single();
					break;

				default:
					sms = null;
					_db.Transaction.Rollback();
					break;
			}

			return sms;
		}

		public void DequeueCommit(bool processed)
		{
			Guid? tid;

			tid = _transactionId;

			_db.SmsQueue_Dequeue_Commit(ref tid, processed);

			if (processed)
				_db.Transaction.Commit();
			else
				_db.Transaction.Rollback();

			_db.Connection.Close();
		}


		#region IDisposable Members

		public void Dispose()
		{
			if (_db != null)
			{
				if (_db.Connection.State != ConnectionState.Closed)
				{
					_db.Connection.Close();
				}
				_db.Dispose();
				_db = null;
			}
		}

		#endregion
	}
}
