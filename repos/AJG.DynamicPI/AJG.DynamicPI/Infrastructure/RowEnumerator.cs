using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AJG.TravelApp.Web.Admin.Infrastructure
{
    /// <summary>
    /// These specilised classes was resulted from optimisation efforts and certain EntityFramework limitations.
    /// </summary>
    public abstract class RowEnumerator<T> : IEnumerator<T>, IDisposable
    {
        private readonly SqlDataReader mDr;
        private T mCurr;

        public RowEnumerator(SqlDataReader dr)
        {
            mDr = dr;
            mCurr = default(T);
        }

        public bool MoveNext()
        {
            if(!mDr.Read()) return false;

            mCurr = Fill(mDr);

            return true;
        }

        T IEnumerator<T>.Current => mCurr;

        public object Current => this.mCurr;

        public void Reset()
        {
            throw new NotSupportedException();
        }


        public abstract T Fill(SqlDataReader dr);


        #region IDisposable

        bool mDisposed = false;

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if(mDisposed) return;

            if(disposing)
            {
                // Free any other managed objects here. 

                using(mDr) { }
            }

            // Free any unmanaged objects here. 
            // ...

            mDisposed = true;
        }

        #endregion
    }
}