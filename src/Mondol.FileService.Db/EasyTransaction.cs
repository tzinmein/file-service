using System;
using System.Data;

namespace Dapper
{
    /// <summary>
    ///     Transaction object helps maintain transaction depth counts
    /// </summary>
    public class EasyTransaction : IDisposable
    {
        private IDbTransaction _trans;

        public EasyTransaction(IDbConnection dbConn, IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            _trans = dbConn.BeginTransaction(isolation);
        }

        public void Complete()
        {
            _trans.Commit();
            _trans = null;
        }

        public void Dispose()
        {
            if (_trans != null)
            {
                _trans.Rollback();
                _trans = null;
            }
        }
    }
}
