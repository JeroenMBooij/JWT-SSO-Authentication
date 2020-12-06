using AuthenticationServer.Common.Models.ResponseModels;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repository
{
    public interface IUserRepository : IRepository
    {
        Task<JwtConfiguration> CreateUser<UserDTO>(UserDTO data);
        Task<JwtConfiguration> CreateTenant(Tenant tenant);
    }
}
