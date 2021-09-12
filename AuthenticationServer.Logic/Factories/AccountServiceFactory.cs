using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Logic.Services.Account;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Factories
{
    public class AccountServiceFactory : IAccountServiceFactory
    {
        private readonly IMapper _mapper;
        private readonly IJwtManager _jwtManager;
        private readonly ITenantAccountRepository _tenantRepo;
        private readonly IAdminAccountRepository _applicationAccountRepo;
        private readonly ILanguageRepository _languageRepo;
        private readonly IApplicationRepository _applicationRepo;
        private readonly IJwtTenantConfigRepository _jwtTenantConfigRepo;
        private readonly IEmailManager _emailManager;
        private readonly IConfiguration _config;


        private Dictionary<string, IAccountService> _services;


        // TODO look into Mediator for constructor parameters
        public AccountServiceFactory(IMapper mapper, IJwtManager jwtManager, ITenantAccountRepository tenantRepo, IAdminAccountRepository applicationAccountRepo,
            ILanguageRepository languageRepo, IApplicationRepository applicationRepo, IJwtTenantConfigRepository jwtTenantConfigRepo,
            IEmailManager emailManager, IConfiguration config)
        {
            _mapper = mapper;
            _jwtManager = jwtManager;
            _tenantRepo = tenantRepo;
            _applicationAccountRepo = applicationAccountRepo;
            _languageRepo = languageRepo;
            _applicationRepo = applicationRepo;
            _jwtTenantConfigRepo = jwtTenantConfigRepo;
            _emailManager = emailManager;
            _config = config;

            _services = CreateTypeMap();
        }

        public async Task<IAccountService> CreateAccountService(string email)
        {
            AccountRole role = await IdentifyAccount(email);

            return CreateAccountService(role);
        }

        public async Task<IAccountService> CreateAccountService(Guid Id)
        {
            AccountRole role = await IdentifyAccount(Id);

            return CreateAccountService(role);
        }

        public IAccountService CreateAccountService(AccountRole accountRole)
        {
            if (_services.TryGetValue(accountRole.ToString(), out IAccountService accountServiceInstance))
                return accountServiceInstance;

            return null;
        }


        public async Task<AccountRole> IdentifyAccount(string email)
        {
            return await _applicationRepo.GetAccountRoleFromEmail(email);
        }

        public async Task<AccountRole> IdentifyAccount(Guid id)
        {
            return await _applicationRepo.GetAccountRoleFromId(id);
        }

        private Dictionary<string, IAccountService> CreateTypeMap()
        {
            Dictionary<string, IAccountService> typeMap = new Dictionary<string, IAccountService>();

            var tenantService = new TenantAccountService(_mapper, _jwtManager, _tenantRepo, _languageRepo,
                _applicationRepo, _jwtTenantConfigRepo, _emailManager);

            typeMap.Add(AccountRole.Tenant.ToString(), tenantService);

            var adminService = new AdminAccountService(_mapper, _applicationAccountRepo, _languageRepo, _applicationRepo,
                _config, _jwtManager, _emailManager);

            typeMap.Add(AccountRole.Admin.ToString(), adminService);

            return typeMap;
        }
    }
}
