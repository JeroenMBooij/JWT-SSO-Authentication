using AuthenticationServer.Common.Models.ContractModels;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface ITenantAccountService
    {
        Task<string> RegisterTenantAsync(Tenant tenant);
    }
}
