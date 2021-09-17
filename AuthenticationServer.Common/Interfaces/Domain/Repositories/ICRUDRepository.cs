using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface ICRUDRepository<T>
    {
        Task<Guid> Insert(T entity, string data = "");

        Task<T> Get(Guid? adminId, Guid id);

        Task<List<T>> GetAll(Guid adminId);

        Task Update(Guid adminId, Guid id, T jwtTenantConfigEntity);

        Task Delete(Guid adminId, Guid id);
    }
}