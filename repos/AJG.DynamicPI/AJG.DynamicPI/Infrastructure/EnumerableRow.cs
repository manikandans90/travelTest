using System.Collections.Generic;

namespace AJG.TravelApp.Web.Admin.Infrastructure
{
    /// <summary>
    /// These specilised classes was resulted from optimisation efforts and certain EntityFramework limitations.
    /// </summary>
    public class EnumerableRow<T> : IEnumerable<T>
    {
        private readonly RowEnumerator<T> mRe;

        public EnumerableRow(RowEnumerator<T> re)
        {
            mRe = re;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return mRe;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mRe;
        }
    }
}