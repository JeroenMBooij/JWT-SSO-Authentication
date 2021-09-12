using AuthenticationServer.Common.Constants.AccountManager;
using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.ContractModels.Applications;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AuthenticationServer.Logic.Factories;
using AuthenticationServer.Logic.Managers.Account;
using AutoMapper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AuthenticationServer.Service.Application
{
    public class ApplicationAccountService : IApplicationAccountService
    {
        private readonly IMapper _mapper;
        private readonly IApplicationAccountRepository _applicationRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtManager _jwtManager;
        private readonly IEmailManager _emailManager;
        private readonly AccountManager _accountManager;

        public ApplicationAccountService(IMapper mapper, IApplicationAccountRepository applicationRepository, IAccountRepository accountRepository,
            IJwtManager jwtManager, IAccountManagerFactory accountManagerFactory, IEmailManager emailManager)
        {
            _mapper = mapper;
            _applicationRepository = applicationRepository;
            _accountRepository = accountRepository;
            _jwtManager = jwtManager;
            _emailManager = emailManager;
            _accountManager = accountManagerFactory.CreateAccountManager(AMName.Application);
        }

        public async Task<string> LoginAsync(Credentials credentials)
        {
            AccountDto adminDto = await _accountManager.LoginAsync(credentials.Email, credentials.Password);

            JwtModelDto jwtConfigurationDto = await _accountManager.CreateJwtConfigurationAsync(null, adminDto);

            string token = _jwtManager.GenerateToken(jwtConfigurationDto);

            return token;
        }

        public async Task<string> RegisterAsync(ApplicationAccount applicationAccount)
        {
            if (applicationAccount.Token != null)
            {
                List<ApplicationDto> applicationsDto = _mapper.Map<List<ApplicationDto>>(applicationAccount.Assets);

                AccountDto accountDto = await _jwtManager.GetApplicationUserDto(applicationAccount.Token);

                foreach(ApplicationDto applicationDto in applicationsDto)
                {
                    applicationDto.Tenant = accountDto;
                    applicationDto.Id = accountDto.Id;

                    await _applicationRepository.Insert(_mapper.Map<ApplicationEntity>(applicationDto));
                }    
            }
            else
            {

                AccountDto adminAccountDto = _mapper.Map<AccountDto>(applicationAccount);

                await _accountManager.CreateAccountAsync(adminAccountDto);

                if(Debugger.IsAttached == false)
                    await _emailManager.SendVerificationEmail(adminAccountDto.Email, adminAccountDto.Id);
                else
                    await _accountRepository.SetVerified(adminAccountDto.Id);

                return $"An Email has been send to {adminAccountDto.Email}. Please confirm your Email to complete your registration.";
            }

            return "success";
        }

        public async Task UpdateApplicationAsync(ApplicationWithId application)
        {
            ApplicationDto applicationDto = _mapper.Map<ApplicationDto>(await _applicationRepository.Get(application.Id));
            if (applicationDto is null)
                throw new AuthenticationApiException("update authentication", "Application not found", 404);

            ApplicationDto mergedApplicationDto = _mapper.Map(application, applicationDto);

            await _applicationRepository.Update(applicationDto.Id, _mapper.Map<ApplicationEntity>(mergedApplicationDto));
        }
    }
}
