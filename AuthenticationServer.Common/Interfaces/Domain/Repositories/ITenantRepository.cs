using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
            
namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface ITenantRepository
    {
        Task Insert(TenantEntity tenant);
        Task<TenantEntity> Get(Guid Id);
        Task<List<TenantEntity>> GetAll();
        Task Update(TenantEntity tenant);
        Task Delete(TenantEntity tenant);


        Task<JwtConfigurationDto> GetTenantJwtConfiguration(TenantEntity tenantDTO);
    }
}
