using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Handlers;
using AuthenticationServer.Logic.Managers.Account;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AuthenticationServer.Logic.Factories
{
    public class AccountManagerFactory : IAccountManagerFactory
    {
        private readonly IMapper _mapper;
        private readonly ITenantRepository _tenantRepository;
        private readonly IDomainHandler _domainHandler;
        private readonly IJwtConfigurationRepository _jwtConfigurationRepository;

        private Dictionary<string, Type> _boards;

        public AccountManagerFactory(IMapper mapper, ITenantRepository tenantRepository,
            IDomainHandler domainHandler, IJwtConfigurationRepository jwtConfigurationRepository)
        {
            _boards = CreateTypeMap();
            _mapper = mapper;
            _tenantRepository = tenantRepository;
            _domainHandler = domainHandler;
            _jwtConfigurationRepository = jwtConfigurationRepository;
        }

        public AccountManager CreateAccountManager(string manager)
        {

            if (_boards.TryGetValue(manager, out Type accountManager))
                return Activator.CreateInstance(accountManager, _mapper, _tenantRepository, _domainHandler, _jwtConfigurationRepository) as AccountManager;

            return null;
        }

        private Dictionary<string, Type> CreateTypeMap()
        {
            Dictionary<string, Type> typeMap = new Dictionary<string, Type>();

            Type baseType = typeof(AccountManager);
            Assembly assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes())
            {
                if (baseType.IsAssignableFrom(type) == false || type.IsAbstract)
                    continue;

                AccountManager derivedObject = Activator.CreateInstance(type, _mapper, _tenantRepository, _domainHandler, _jwtConfigurationRepository) as AccountManager;

                if (derivedObject != null)
                {
                    string name = type.Name.Substring(0, type.Name.IndexOf("AccountManager"));
                    typeMap.Add(name, derivedObject.GetType());
                }
            }

            return typeMap;
        }
    }
}
