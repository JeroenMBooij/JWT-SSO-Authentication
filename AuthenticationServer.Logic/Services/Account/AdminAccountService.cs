using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.ContractModels.Applications;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Services.Account
{
    public class AdminAccountService : AbstractAccountService, IAdminAccountService
    {
        private readonly IApplicationRepository _applicationRepo;
        private readonly IConfiguration _config;
        private readonly IJwtManager _jwtManager;

        public AdminAccountService(IMapper mapper, IAdminAccountRepository applicationAccountRepo, ILanguageRepository languageRepository,
            IApplicationRepository applicationRepo, IConfiguration config, IJwtManager jwtManager, IEmailManager emailManager)
            : base(mapper, emailManager, applicationAccountRepo, languageRepository)
        {
            _applicationRepo = applicationRepo;
            _config = config;
            _jwtManager = jwtManager;
        }

        public async Task<string> LoginAsync(Credentials credentials)
        {
            AccountDto adminDto = await LoginAsync(credentials.Email, credentials.Password);

            JwtModelDto jwtConfigurationDto = await CreateJwtConfigurationAsync(null, adminDto);

            string token = _jwtManager.GenerateToken(jwtConfigurationDto);

            return token;
        }

        public async Task<string> RegisterAsync(AdminAccount applicationAccount)
        {
            if (applicationAccount.Token != null)
            {
                List<ApplicationDto> applicationsDto = _mapper.Map<List<ApplicationDto>>(applicationAccount.Assets);

                Guid userId = _jwtManager.GetUserId(applicationAccount.Token);
                AccountDto accountDto = await Get(userId);



                foreach (ApplicationDto applicationDto in applicationsDto)
                {
                    applicationDto.Tenant = accountDto;
                    applicationDto.Id = accountDto.Id;

                    await _applicationRepo.Insert(_mapper.Map<ApplicationEntity>(applicationDto));
                }
            }
            else
            {

                AccountDto adminAccountDto = _mapper.Map<AccountDto>(applicationAccount);

                await CreateAccountAsync(adminAccountDto);

                if (Debugger.IsAttached == false)
                    await _emailManager.SendVerificationEmail(adminAccountDto.Email, adminAccountDto.Id);
                else
                    await _accountRepository.SetVerified(adminAccountDto.Id);

                return $"An Email has been send to {adminAccountDto.Email}. Please confirm your Email to complete your registration.";
            }

            return "success";
        }

        public async Task UpdateApplicationAsync(ApplicationWithId application)
        {
            ApplicationDto applicationDto = _mapper.Map<ApplicationDto>(await _applicationRepo.Get(application.Id));
            if (applicationDto is null)
                throw new AuthenticationApiException("update authentication", "Application not found", 404);

            ApplicationDto mergedApplicationDto = _mapper.Map(application, applicationDto);

            await _applicationRepo.Update(applicationDto.Id, _mapper.Map<ApplicationEntity>(mergedApplicationDto));
        }


        protected override Task<JwtModelDto> CreateJwtConfigurationAsync(Guid? applicationId, AccountDto accountDto)
        {
            JwtModelDto model = _config.GetSection("TenantJwtConfiguration")
                                                                .Get<JwtModelDto>();

            return Task.FromResult(model);
        }

        protected override async Task PopulateAccountPropertiesForNewAccountAsync(AccountDto adminDto)
        {
            adminDto.Id = Guid.NewGuid();
            adminDto.AuthenticationRole = AccountRole.Admin.ToString();
            adminDto.LockoutEnabled = true;

            foreach (ApplicationDto applicationDto in adminDto.Assets)
            {
                applicationDto.Id = Guid.NewGuid();
                applicationDto.AdminId = adminDto.Id;
                applicationDto.Tenant = adminDto;

                foreach (DomainNameDto domainDto in applicationDto.Domains)
                {
                    domainDto.Id = Guid.NewGuid();
                    domainDto.Application = applicationDto;
                }

                foreach (JwtTenantConfigDto jwtTenantconfig in applicationDto.JwtTenantConfigurations)
                {
                    jwtTenantconfig.Id = Guid.NewGuid();
                    jwtTenantconfig.Application = applicationDto;
                }
            }

            await PopulateLanguagePropertiesForNewAccountAsync(adminDto);
        }
    }
}
