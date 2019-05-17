using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data
{
    /// <summary>
    /// Transaction scope.
    /// </summary>
    public class TransactionScope : IDisposable
    {
        bool _isDisposed;

        private readonly DataContext _context;
        private readonly Guid _transguid;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.Data.TransactionScope"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="transguid">Transguid.</param>
        internal TransactionScope(DataContext context, Guid transguid)
        {
            _context = context;
            _transguid = transguid;
        }
        /// <summary>
        /// Checks the trans.
        /// </summary>
        /// <returns><c>true</c>, if trans was checked, <c>false</c> otherwise.</returns>
        public bool CheckTrans()
        {
            return _context.ScopeCheckTrans(_transguid);
        }
        /// <summary>
        /// Releases all resource used by the <see cref="T:Light.Data.TransactionScope"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Close this instance.
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
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

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:Light.Data.TransactionScope"/> is reclaimed by garbage collection.
        /// </summary>
        ~TransactionScope()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

    }
}
