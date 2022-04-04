using AuthenticationServer.Common.Models.ContractModels.Account;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface ITenantAccountManager : IAccountService
    {
        Task<AccountConfirmation> RegisterWithTokenAsync(AccountData tenant, string adminToken);
        Task<AccountConfirmation> RegisterWithHostnameAsync(AccountData tenant, string hostname);
        bool IsTokenValid(string token);
    }
}
