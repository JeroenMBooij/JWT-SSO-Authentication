using AuthenticationServer.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface IJwtConfigurationRepository
    {
        Task Insert(JwtConfigurationEntity jwtConfigurationEntity);
        Task<JwtConfigurationEntity> Get(string Id);
        Task<List<JwtConfigurationEntity>> GetAll();
        Task Update(JwtConfigurationEntity jwtConfigurationEntity);
        Task Delete(JwtConfigurationEntity jwtConfigurationEntity);


        Task CreateJwtConfiguration(JwtConfigurationEntity jwtConfigurationEntity);
        JwtConfigurationEntity GetTenantJwtContainerModel();
    }
}