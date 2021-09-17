using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.ContractModels.Token;
using AuthenticationServer.Common.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Ticket> LoginAsync(Credentials credentials, Guid applicationId);
        Task<Ticket> LoginAsync(Credentials credentials, string hostname = "");
        Task<string> RegisterAsync(AccountRegistration applicationAccount);
        Task<AbstractAccountDto> Get(Guid id);
        Task VerifyEmail(string code);
        Task ChangePassword(NewCredentials newCredentials);
        Task RecoverPassword(ResetPasswordModel resetPasswordModel);
        Task ResetPassword(string email);
        Task<JwtModelDto> CreateJwtModelAsync<T>(Guid? applicationId, T accountDto) where T : AbstractAccountDto;
    }
}