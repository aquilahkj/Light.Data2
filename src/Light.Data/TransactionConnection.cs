using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    internal class TransactionConnection : IDisposable
    {
        private DbTransaction _transaction;

        private DbConnection _connection;

        public bool IsOpen { get; private set; }

        public bool ExecuteFlag { get; private set; }

        public SafeLevel Level { get; }

        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConnection"/> class.
        /// </summary>
        /// <param name="connection">Connection.</param>
        /// <param name="level">Level.</param>
        public TransactionConnection(DbConnection connection, SafeLevel level)
        {
            _connection = connection;
            Level = level;
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
            if (Level == SafeLevel.None) {
                _transaction = null;
            }
            else if (Level == SafeLevel.Default) {
                _transaction = _connection.BeginTransaction();
            }
            else {
                IsolationLevel isoLevel;
                switch (Level) {
                    case SafeLevel.Low:
                        isoLevel = IsolationLevel.ReadUncommitted;
                        break;
                    case SafeLevel.High:
                        isoLevel = IsolationLevel.RepeatableRead;
                        break;
                    case SafeLevel.Serializable:
                        isoLevel = IsolationLevel.Serializable;
                        break;
                    default:
                        isoLevel = IsolationLevel.ReadCommitted;
                        break;
                }
                _transaction = _connection.BeginTransaction(isoLevel);
            }
            IsOpen = true;
        }

        /// <summary>
        /// Setups the command.
        /// </summary>
        /// <param name="command">Command.</param>
        public void SetupCommand(DbCommand command)
        {
            if (IsDisposed) {
                throw new LightDataException(SR.TransactionHasClosed);
            }

            if (_transaction != null) {
                command.Transaction = _transaction;
            }
            command.Connection = _connection;
            ExecuteFlag = true;
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
                ExecuteFlag = false;
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
                    ExecuteFlag = false;
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
        public async Task OpenAsync(CancellationToken cancellationToken = default)
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
            if (IsDisposed) {
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
            IsDisposed = true;
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
