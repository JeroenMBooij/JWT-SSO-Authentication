using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.DataAccess.Interfaces
{
    public interface IData
    {
        Task<List<T>> GetAll<T>();
        Task Insert<T>(T data);
    }
}
