using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class DomainRepository : IDomainRepository
    {
        public Task Delete(DomainEntity domain)
        {
            throw new NotImplementedException();
        }

        public Task<DomainEntity> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DomainEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<object> GetDomainFromUrl(string url)
        {
            throw new NotImplementedException();
        }

        public UserSchemaDto GetUserSchemaFromTenantIdAsync(Guid tenantId)
        {
            throw new NotImplementedException();
        }

        public Task Insert(DomainEntity domain)
        {
            throw new NotImplementedException();
        }

        public Task Update(DomainEntity domain)
        {
            throw new NotImplementedException();
        }
    }
}
