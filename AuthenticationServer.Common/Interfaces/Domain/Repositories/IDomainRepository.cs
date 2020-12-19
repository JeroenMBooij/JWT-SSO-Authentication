using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface IDomainRepository
    {
        Task Insert(DomainEntity domain);
        Task<DomainEntity> Get(Guid id);
        Task<List<DomainEntity>> GetAll();
        Task Update(DomainEntity domain);
        Task Delete(DomainEntity domain);
        Task<object> GetDomainFromUrl(string url);
        UserSchemaDto GetUserSchemaFromTenantIdAsync(Guid tenantId);
    }
}
