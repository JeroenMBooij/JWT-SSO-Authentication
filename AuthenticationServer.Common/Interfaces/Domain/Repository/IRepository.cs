using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repository
{
    public interface IRepository
    {
        Task Insert<T>(T data);
        Task<T> Get<T>(string Id);
        Task<List<T>> GetAll<T>();
        Task Update<T>(T data);
        Task Delete<T>(T data);
    }
}
