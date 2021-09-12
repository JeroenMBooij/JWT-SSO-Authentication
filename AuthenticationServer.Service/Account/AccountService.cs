using AuthenticationServer.Common.Constants.AccountManager;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Logic.Factories;
using AuthenticationServer.Logic.Managers.Account;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly AccountManager _accountManager;
        private readonly IEmailManager _emailManager;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountManagerFactory accountManagerFactory, IEmailManager emailManager, 
            IAccountRepository accountRepository, IMapper mapper)
        {
            _accountManager = accountManagerFactory.CreateAccountManager(AMName.Application);
            _emailManager = emailManager;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task ChangePassword(NewCredentials newCredentials)
        {
            await _accountManager.ChangePassword(newCredentials.Email, newCredentials.OldPassword, newCredentials.NewPassword);
        }

        public async Task ResetPassword(string email)
        {
            string passwordRecoverToken = await _accountManager.ResetPassword(email);

            await _emailManager.SendRecoverPasswordEmail(email, passwordRecoverToken);
        }

        public async Task RecoverPassword(ResetPasswordModel resetPasswordModel)
        {
            await _accountManager.RecoverPassword(resetPasswordModel.Email, resetPasswordModel.ResetToken, resetPasswordModel.NewPassword);
        }

        public async Task VerifyEmail(string code)
        {
            AccountDto tenantDto;
            try
            {
                tenantDto = _mapper.Map<AccountDto>(await _accountRepository.Get(Guid.Parse(code)));
            }
            catch (Exception)
            {
                throw new Exception("Bad Request contact your system administrator.");
            }

            await _accountRepository.SetVerified(tenantDto.Id);
        }
    }
}
