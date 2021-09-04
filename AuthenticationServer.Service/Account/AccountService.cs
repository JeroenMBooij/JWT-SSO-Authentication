using AuthenticationServer.Common.Constants.AccountManager;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Logic.Factories;
using AuthenticationServer.Logic.Managers.Account;
using AutoMapper;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AuthenticationServer.Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IJwtManager _jwtManager;
        private readonly ITenantRepository _tenantRepository;
        private readonly AccountManager _accountManager;
        private readonly IEmailManager _emailManager;

        public AccountService(IMapper mapper, IJwtManager jwtManager, ITenantRepository tenantRepository,
            IAccountManagerFactory accountManagerFactory, IEmailManager emailManager)
        {
            _jwtManager = jwtManager;
            _tenantRepository = tenantRepository;
            _accountManager = accountManagerFactory.CreateAccountManager(AMName.Tenant);
            _emailManager = emailManager;
            _mapper = mapper;
        }

        public async Task ChangePassword(NewCredentials newCredentials)
        {
            await _accountManager.ChangePassword(newCredentials.Email, newCredentials.OldPassword, newCredentials.NewPassword);
        }

        public bool IsTokenValid(string token)
        {
            return _jwtManager.IsTokenValid(token);
        }

        public async Task<string> LoginTenantAsync(Credentials credentials)
        {
            AccountDto tenantDto = await _accountManager.LoginAsync(credentials.Email, credentials.Password);

            JwtConfigurationDto jwtConfigurationDto = _accountManager.CreateTenantJwtConfiguration(tenantDto);

            string token = _jwtManager.GenerateToken(jwtConfigurationDto);

            return token;
        }

        public async Task ResetPassword(string email)
        {
            string passwordRecoverToken = await _accountManager.ResetPassword(email);

            await _emailManager.SendRecoverPasswordEmail(email, passwordRecoverToken);
        }

        public async Task<string> RegisterTenantAsync(AccountRegistration tenant, string adminToken)
        {
            AccountDto tenantDto = await _accountManager.CreateAccountAsync(_mapper.Map<AccountDto>(tenant));

            if (Debugger.IsAttached == false)
                await _emailManager.SendVerificationEmail(tenant.Email, tenantDto.Id);
            else
                await _tenantRepository.SetVerified(tenantDto.Id);
            

            return $"An Email has been send to {tenant.Email}. Please confirm your Email to complete your registration.";
        }

        public async Task RecoverPassword(ResetPasswordModel resetPasswordModel)
        {
            await _accountManager.RecoverPassword(resetPasswordModel.Email, resetPasswordModel.ResetToken, resetPasswordModel.NewPassword);
        }
    }
}
