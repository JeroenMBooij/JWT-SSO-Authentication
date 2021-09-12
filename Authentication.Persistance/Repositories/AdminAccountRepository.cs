using AuthenticationServer.Common.Constants.StoredProcedures;
using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Domain.Entities;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class AdminAccountRepository : AbstractAccountRepository, IAdminAccountRepository
    {
        public AdminAccountRepository(IMainSqlDataAccess db, SignInManager<ApplicationUserEntity> signInManager,
            UserManager<ApplicationUserEntity> accountManager)
            : base(db, accountManager, signInManager)
        {
        }

        public async Task AddTenantToAdmin(Guid tenantId, string adminId)
        {
            var parametersToStoredProcedure = new Dictionary<DynamicParameters, string>();

            var tenantLanguageParameters = new DynamicParameters();
            tenantLanguageParameters.Add("TenantId", tenantId);
            tenantLanguageParameters.Add("AdmintId", adminId);
            parametersToStoredProcedure.Add(tenantLanguageParameters, SPName.AddTenant);

            await _db.ExecuteStoredProcedures(parametersToStoredProcedure);
        }

    }
}
