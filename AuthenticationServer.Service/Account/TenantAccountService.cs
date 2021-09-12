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
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Service.Account
{
    public class TenantAccountService : ITenantAccountService
    {
        private readonly IMapper _mapper;
        private readonly IJwtManager _jwtManager;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IEmailManager _emailManager;
        private readonly AccountManager _accountManager;

        public TenantAccountService(IMapper mapper, IJwtManager jwtManager, IApplicationRepository applicationRepository,
            IAccountManagerFactory accountManagerFactory, IEmailManager emailManager)
        {
            _jwtManager = jwtManager;
            _applicationRepository = applicationRepository;
            _emailManager = emailManager;
            _accountManager = accountManagerFactory.CreateAccountManager(AMName.Tenant);
            _mapper = mapper;
        }


        public bool IsTokenValid(string token)
        {
            return _jwtManager.IsTokenValid(null, token);
        }

        public async Task<string> LoginAsync(Credentials credentials, Guid applicationId)
        {
            ApplicationDto applicationDto = _mapper.Map<ApplicationDto>
                (await _applicationRepository.Get(applicationId));

            return await LoginAsync(credentials, applicationDto);
        }

        public async Task<string> LoginAsync(Credentials credentials, string hostname)
        {
            ApplicationDto applicationDto = _mapper.Map<ApplicationDto>
                (await _applicationRepository.GetApplicationFromHostname(hostname));

            return await LoginAsync(credentials, applicationDto);
        }

        private async Task<string> LoginAsync(Credentials credentials, ApplicationDto applicationDto)
        {
            AccountDto tenantDto = await _accountManager.LoginAsync(credentials.Email, credentials.Password);

            JwtModelDto jwtConfigurationDto = await _accountManager.CreateJwtConfigurationAsync(applicationDto.Id, tenantDto);

            string token = _jwtManager.GenerateToken(jwtConfigurationDto);

            return token;
        }


        public async Task<string> RegisterWithHostnameAsync(AccountRegistration tenant, string hostname)
        {
            ApplicationDto applicationDto = _mapper.Map<ApplicationDto>(await _applicationRepository.GetApplicationFromHostname(hostname));

            AccountDto tenantAccount = _mapper.Map<AccountDto>(tenant);
            tenantAccount.AdminId = applicationDto.AdminId;
            //TODO multiple jwt configurations on 1 application identifier
            tenantAccount.LockoutEnabled = applicationDto.JwtTenantConfigurations.FirstOrDefault().LockoutEnabled;

            return await RegisterTenantAsync(tenantAccount);
        }

        public async Task<string> RegisterWithTokenAsync(AccountRegistration tenant, string adminToken)
        {
            Guid adminId = _jwtManager.GetUserId(adminToken);

            AccountDto tenantAccount = _mapper.Map<AccountDto>(tenant);
            tenantAccount.AdminId = adminId;
            tenantAccount.LockoutEnabled = true;

            return await RegisterTenantAsync(tenantAccount);
        }

        private async Task<string> RegisterTenantAsync(AccountDto AccountDto)
        {
            AccountDto = await _accountManager.CreateAccountAsync(AccountDto);

            if (Debugger.IsAttached == false)
                await _emailManager.SendVerificationEmail(AccountDto.Email, AccountDto.Id);
            else
                await _accountRepository.SetVerified(AccountDto.Id);
            

            return $"An Email has been send to {AccountDto.Email}. Please confirm your Email to complete your registration.";
        }

    }
}
