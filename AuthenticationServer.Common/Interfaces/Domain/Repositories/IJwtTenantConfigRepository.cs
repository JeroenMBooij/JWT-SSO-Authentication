using AuthenticationServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface IJwtTenantConfigRepository: ICRUDRepository<JwtTenantConfigEntity>
    {
        Task CreateJwtConfiguration(JwtTenantConfigEntity jwtConfigurationEntity);
        Task<JwtTenantConfigEntity> GetFromApplicationId(Guid applicationId);
    }
}