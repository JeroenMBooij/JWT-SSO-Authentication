using AuthenticationServer.Common.Interfaces.Domain.Repository;
using AuthenticationServer.Common.Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Common.Interfaces.Domain.Repositories
{
    public interface IJwtConfigurationRepository : IRepository
    {
        Task CreateJwtConfiguration(JwtConfiguration tenant, int domainId);
    }
}