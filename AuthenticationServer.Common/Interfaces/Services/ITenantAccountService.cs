using AuthenticationServer.Common.Models.ContractModels.Account;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface ITenantAccountService : IAccountService
    {
        Task<string> RegisterWithTokenAsync(AccountData tenant, string adminToken);
        Task<string> RegisterWithHostnameAsync(AccountData tenant, string hostname);
        bool IsTokenValid(string token);
    }
}
