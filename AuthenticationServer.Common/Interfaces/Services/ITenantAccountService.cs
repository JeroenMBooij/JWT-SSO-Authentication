using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface ITenantAccountService: IAccountService
    {
        Task<string> RegisterWithTokenAsync(AccountRegistration tenant, string adminToken);
        Task<string> RegisterWithHostnameAsync(AccountRegistration tenant, string hostname);
        Task<string> LoginAsync(Credentials credentials, Guid applicationId);
        Task<string> LoginAsync(Credentials credentials, string hostname);
        bool IsTokenValid(string token);
    }
}
