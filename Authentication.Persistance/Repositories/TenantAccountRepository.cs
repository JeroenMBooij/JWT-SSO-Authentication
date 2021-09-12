using AuthenticationServer.Common.Constants.StoredProcedures;
using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Common;
using AuthenticationServer.Domain.Entities;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class TenantAccountRepository : AbstractAccountRepository, ITenantAccountRepository
    {
        public TenantAccountRepository(IMainSqlDataAccess db, SignInManager<ApplicationUserEntity> signInManager, 
            UserManager<ApplicationUserEntity> accountManager)
            :base(db, accountManager, signInManager)
        { 
        }

        public Task<JwtModelDto> GetTenantJwtConfiguration(ApplicationUserEntity tenantEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}
