using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data
{
    public class TransactionScope : IDisposable
    {
        bool _isDisposed;

        private readonly DataContext _context;
        private readonly Guid _transguid;

        internal TransactionScope(DataContext context, Guid transguid)
        {
            _context = context;
            _transguid = transguid;
        }

        public bool CheckTrans() {
            return _context.ScopeCheckTrans(_transguid);
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

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed) {
                if (disposing) {
                    // dispose managed state (managed objects).
                    _context.ScopeCloseTrans(_transguid);
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
