using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain
{
    public interface ISqlDataAccess
    {
        string Connectionstring { get; set; }
        Task<T> GetData<T, U>(string sql, U parameters);
        Task<List<T>> GetAllData<T, U>(string sql, U parameters);
        Task SaveData<T>(string sql, T parameters);
    }
}