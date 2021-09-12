using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AccountDto> Get(Guid id);
        Task VerifyEmail(string code);
        Task ChangePassword(NewCredentials newCredentials);
        Task RecoverPassword(ResetPasswordModel resetPasswordModel);
        Task ResetPassword(string email);
    }
}