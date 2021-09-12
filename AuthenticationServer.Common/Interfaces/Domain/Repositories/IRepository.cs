using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface IRepository<T>
    {
        Task Insert(T entity, string data = "");
        Task<T> Get(Guid id);
        Task<List<T>> GetAll();
        Task Update(Guid id, T entity);
        Task Delete(T entity);
    }
}
