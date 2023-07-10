using System.Data;

namespace Dapper
{
    public static class DbConnectionExtensions
    {
        public static EasyTransaction Transaction(this IDbConnection dbConn, IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            return new EasyTransaction(dbConn, isolation);
        }
    }
}
