using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface IAccountService
    {
        Task<string> RegisterTenantAsync(AccountRegistration tenant, string adminToken);
        Task<string> LoginTenantAsync(Credentials credentials);
        bool IsTokenValid(string token);
        Task ChangePassword(NewCredentials newCredentials);
        Task ResetPassword(string email);
        Task RecoverPassword(ResetPasswordModel resetPasswordModel);
    }
}
