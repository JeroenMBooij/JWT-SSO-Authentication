using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Exceptions;
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
        private readonly IAdminAccountRepository _adminAccountRepo;
        private readonly IApplicationRepository _applicationRepo;
        private readonly IJwtTenantConfigRepository _jwtTenantConfigRepo;
        private readonly IEmailManager _emailManager;
        private readonly IConfiguration _config;


        private Dictionary<AccountRole, IAccountService> _services;


        // TODO look into Mediator for constructor parameters
        public AccountServiceFactory(IMapper mapper, IJwtManager jwtManager, ITenantAccountRepository tenantRepo, IAdminAccountRepository applicationAccountRepo,
            IApplicationRepository applicationRepo, IJwtTenantConfigRepository jwtTenantConfigRepo,
            IEmailManager emailManager, IConfiguration config)
        {
            _mapper = mapper;
            _jwtManager = jwtManager;
            _tenantRepo = tenantRepo;
            _adminAccountRepo = applicationAccountRepo;
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
            if (_services.TryGetValue(accountRole, out IAccountService accountServiceInstance))
                return accountServiceInstance;

            return null;
        }


        public async Task<AccountRole> IdentifyAccount(string email)
        {
            AccountRole? accountRole = await _applicationRepo.GetAccountRole(email);

            if (accountRole == null)
                throw new AuthenticationApiException("login", "Invalid Credentials provided");

            return accountRole.Value;
        }

        public async Task<AccountRole> IdentifyAccount(Guid id)
        {
            AccountRole? accountRole = await _applicationRepo.GetAccountRole(id);

            if (accountRole == null)
                throw new AuthenticationApiException("verify", "Invalid AccountId provided");

            return accountRole.Value;
        }

        private Dictionary<AccountRole, IAccountService> CreateTypeMap()
        {
            Dictionary<AccountRole, IAccountService> typeMap = new Dictionary<AccountRole, IAccountService>();

            var tenantService = new TenantAccountService(_mapper, _jwtManager, _tenantRepo,
                _applicationRepo, _jwtTenantConfigRepo, _emailManager);

            typeMap.Add(AccountRole.Tenant, tenantService);

            var adminService = new AdminAccountService(_mapper, _jwtManager, _adminAccountRepo,
                _config, _emailManager);

            typeMap.Add(AccountRole.Admin, adminService);

            return typeMap;
        }
    }
}
