using AuthenticationServer.Common.Models.ResponseModels;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface IRegisterService
    {
        Task<string> RegisterUser(User user);
        Task<string> RegisterTenant(Tenant tenant);
    }
}