using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.DataAccess
{
    public interface IMainSqlDataAccess
    {
        string Connectionstring { get; set; }
        Task<T> GetData<T, U>(string sql, U parameters);
        Task<List<T>> GetAllData<T, U>(string sql, U parameters);
        Task ExecuteStoredProcedures<T>(Dictionary<T, string> parametersToStoredProcedure);
        Task SaveData<T, U>(string sql, U parameters);
    }
}