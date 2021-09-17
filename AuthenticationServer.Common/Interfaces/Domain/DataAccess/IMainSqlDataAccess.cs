using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.DataAccess
{
    public interface IMainSqlDataAccess : IDisposable
    {
        // Dapper supports up to 7 entities. Up to 4 entities have been implemented
        Task<TReturn> GetData<TReturn, U>(string sql, U parameters);
        Task<TReturn> GetData<TFirst, TSecond, TReturn, U>(string sql, U parameters, Func<TFirst, TSecond, TReturn> mapper);
        Task<TReturn> GetData<TFirst, TSecond, TThird, TReturn, U>(string sql, U parameters, Func<TFirst, TSecond, TThird, TReturn> mapper);
        Task<TReturn> GetData<TFirst, TSecond, TThird, TFourth, TReturn, U>(string sql, U parameters, Func<TFirst, TSecond, TThird, TFourth, TReturn> mapper);
        Task<List<TReturn>> GetAllData<TReturn, U>(string sql, U parameters);
        Task<List<TReturn>> GetAllData<TFirst, TSecond, TReturn, U>(string sql, U parameters, Func<TFirst, TSecond, TReturn> mapper);
        Task<List<TReturn>> GetAllData<TFirst, TSecond, TThird, TReturn, U>(string sql, U parameters, Func<TFirst, TSecond, TThird, TReturn> mapper);
        Task<List<TReturn>> GetAllData<TFirst, TSecond, TThird, TFourth, TReturn, U>(string sql, U parameters, Func<TFirst, TSecond, TThird, TFourth, TReturn> mapper);
        Task ExecuteStoredProcedures<T>(Dictionary<T, string> parametersToStoredProcedure);
        Task SaveData<U>(string sql, U parameters);

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}