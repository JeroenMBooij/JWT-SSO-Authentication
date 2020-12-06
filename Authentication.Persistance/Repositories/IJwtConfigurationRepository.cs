using AuthenticationServer.Common.Interfaces.Domain.Repository;
using AuthenticationServer.Common.Models.ResponseModels;
using AuthenticationServer.Common.Models.ResponseModels.Common;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public interface IJwtConfigurationRepository : IRepository
    {
        Task CreateJwtConfiguration(JwtConfiguration tenant, int domainId);
        Task<JwtConfiguration> GetJwtConfigurationFromUser(Account account);
    }
}