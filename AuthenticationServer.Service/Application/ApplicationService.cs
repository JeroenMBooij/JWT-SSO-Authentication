using AuthenticationServer.Common.Constants.AccountManager;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AuthenticationServer.Logic.Factories;
using AuthenticationServer.Logic.Managers.Account;
using AutoMapper;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AuthenticationServer.Service.Application
{
    public class ApplicationService : IApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IJwtManager _jwtManager;
        private readonly IEmailManager _emailManager;
        private readonly AccountManager _accountManager;

        public ApplicationService(IMapper mapper, IApplicationRepository applicationRepository, ITenantRepository tenantRepository,
            IJwtManager jwtManager, IAccountManagerFactory accountManagerFactory, IEmailManager emailManager)
        {
            _mapper = mapper;
            _applicationRepository = applicationRepository;
            _tenantRepository = tenantRepository;
            _jwtManager = jwtManager;
            _emailManager = emailManager;
            _accountManager = accountManagerFactory.CreateAccountManager(AMName.Application);
        }

        public async Task<string> RegisterApplicationAsync(ApplicationAccount applicationAccount)
        {
            if (applicationAccount.Token != null)
            {
                ApplicationDto applicationDto = _mapper.Map<ApplicationDto>(applicationAccount.Asset);

                AccountDto accountDto = await _jwtManager.GetApplicationUserDto(applicationAccount.Token);
                applicationDto.Tenant = accountDto;
                applicationDto.Id = accountDto.Id;

                await _applicationRepository.Insert(_mapper.Map<ApplicationEntity>(applicationDto));
            }
            else
            {

                AccountDto adminAccountDto = _mapper.Map<AccountDto>(applicationAccount);

                await _accountManager.CreateAccountAsync(adminAccountDto);

                if(Debugger.IsAttached == false)
                    await _emailManager.SendVerificationEmail(adminAccountDto.Email, adminAccountDto.Id);
                else
                    await _tenantRepository.SetVerified(adminAccountDto.Id);

                return $"An Email has been send to {adminAccountDto.Email}. Please confirm your Email to complete your registration.";
            }

            return "success";
        }
    }
}
