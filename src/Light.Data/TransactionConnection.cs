using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
	class TransactionConnection : IDisposable
	{
		DbTransaction _transaction;

		DbConnection _connection;

		SafeLevel _level = SafeLevel.Default;

		bool _isOpen;

		public bool IsOpen {
			get {
				return _isOpen;
			}
		}

		public SafeLevel Level {
			get {
				return _level;
			}
		}

		bool _isDisposed;

		public bool IsDisposed {
			get {
				return _isDisposed;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionConnection"/> class.
		/// </summary>
		/// <param name="connection">Connection.</param>
		/// <param name="level">Level.</param>
		public TransactionConnection(DbConnection connection, SafeLevel level)
		{
			_connection = connection;
			_level = level;
		}

		///// <summary>
		///// Resets the transaction.
		///// </summary>
		///// <param name="level">Level.</param>
		//public void ResetTransaction(SafeLevel level) {
		//	_level = level;
		//	SetupTransaction();
		//}

		private void SetupTransaction()
		{
			if (_transaction != null) {
				_transaction.Dispose();
			}
			if (_level == SafeLevel.None) {
				_transaction = null;
			}
			else if (_level == SafeLevel.Default) {
				_transaction = _connection.BeginTransaction();
			}
			else {
				IsolationLevel isoLevel;
				switch (_level) {
					case SafeLevel.Low:
						isoLevel = IsolationLevel.ReadUncommitted;
						break;
					case SafeLevel.High:
						isoLevel = IsolationLevel.RepeatableRead;
						break;
					default:
						isoLevel = IsolationLevel.ReadCommitted;
						break;
				}
				_transaction = _connection.BeginTransaction(isoLevel);
			}
			_isOpen = true;
		}

		/// <summary>
		/// Setups the command.
		/// </summary>
		/// <param name="command">Command.</param>
		public void SetupCommand(DbCommand command)
		{
			if (_transaction != null) {
				command.Transaction = _transaction;
			}
			command.Connection = _connection;
		}

		/// <summary>
		/// Open this transaction collection.
		/// </summary>
		public void Open()
		{
			_connection.Open();
			SetupTransaction();
		}

		/// <summary>
		/// Commit this transaction collection.
		/// </summary>
		public void Commit()
		{
			if (_transaction != null) {
				_transaction.Commit();
			}
		}

		/// <summary>
		/// Rollback this transaction collection.
		/// </summary>
		public void Rollback()
		{
			if (_transaction != null) {
				try {
					_transaction.Rollback();
				}
				catch {

				}
			}
		}


		#region async
		/// <summary>
		/// Async open this transaction collection..
		/// </summary>
		/// <returns>The async.</returns>
		public async Task OpenAsync(CancellationToken cancellationToken)
		{
			await _connection.OpenAsync(cancellationToken);
			SetupTransaction();
		}


		#endregion

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		private void Dispose(bool disposing)
		{
			if (_isDisposed) {
				return;
			}

			if (disposing) {
				if (_connection != null) {
					_connection.Dispose();
					_connection = null;
				}
				if (_transaction != null) {
					_transaction.Dispose();
					_transaction = null;
				}
			}
			_isDisposed = true;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="TransactionConnection"/> object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="TransactionConnection"/> is reclaimed by garbage collection.
		/// </summary>
		~TransactionConnection()
		{
			Dispose(false);//释放非托管资源
		}
	}
}
