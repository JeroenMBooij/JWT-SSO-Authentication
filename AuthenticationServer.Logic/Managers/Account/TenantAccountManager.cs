using AuthenticationServer.Common.Constants.AccountManager;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Handlers;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Managers.Account
{
    public class TenantAccountManager : AccountManager
    {
        public TenantAccountManager(IMapper mapper, ITenantRepository tenantRepository, IDomainHandler domainHandler, IJwtConfigurationRepository jwtConfigurationRepository) 
            :base(mapper, tenantRepository, domainHandler, jwtConfigurationRepository)
        {
        }

        protected override async Task PopulateAccountPropertiesForNewAccountAsync(AccountDto tenantDto)
        {
            tenantDto.Id = Guid.NewGuid();
            tenantDto.AuthenticationRole = Roles.Tenant; ;


            await PopulateLanguagePropertiesForNewAccountAsync(tenantDto);

            await _domainHandler.AddTenantToAdmin(tenantDto);
        }
    }
}
