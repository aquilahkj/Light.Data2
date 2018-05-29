using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data
{
    public class TransactionScope : IDisposable
    {
        bool _isDisposed;

        DataContext _context;

        public DataContext Context {
            get {
                return _context;
            }
        }

        internal TransactionScope(DataContext context)
        {
            _context = context;
        }

        public bool BeginTrans(SafeLevel level = SafeLevel.Default)
        {
            return _context.BeginTrans(level);
        }

        public bool CommitTrans()
        {
            return _context.CommitTrans();
        }

        public bool RollbackTrans()
        {
            return _context.RollbackTrans(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Close()
        {
            Dispose();
        }

        void CheckStatus()
        {
            if (this._isDisposed) {
                throw new ObjectDisposedException(nameof(TransactionScope));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed) {
                if (disposing) {
                    // dispose managed state (managed objects).
                    _context.CloseTrans(true);
                }
                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.

                _isDisposed = true;
            }
        }

        ~TransactionScope()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

    }
}
