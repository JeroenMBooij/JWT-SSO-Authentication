using AuthenticationServer.Common.Constants.AccountManager;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Handlers;
using AuthenticationServer.Common.Models.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Managers.Account
{
    public class ApplicationAccountManager : AccountManager
    {
        public ApplicationAccountManager(IMapper mapper, ITenantRepository tenantRepository, IDomainHandler domainHandler, IJwtConfigurationRepository jwtConfigurationRepository) 
            : base(mapper, tenantRepository, domainHandler, jwtConfigurationRepository)
        {
        }

        protected override async Task PopulateAccountPropertiesForNewAccountAsync(AccountDto adminDto)
        {
            adminDto.Id = Guid.NewGuid();
            adminDto.AuthenticationRole = Roles.Admin;

            foreach (ApplicationDto applicationDto in adminDto.Assets)
            {
                applicationDto.Id = Guid.NewGuid();
                applicationDto.AdminId = adminDto.Id;
                applicationDto.Tenant = adminDto;
            }


            await PopulateLanguagePropertiesForNewAccountAsync(adminDto);
        }
    }
}
