using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public class TenantAccountRepository : AbstractAccountRepository, ITenantAccountRepository
    {
        public TenantAccountRepository(IMainSqlDataAccess db, SignInManager<ApplicationUserEntity> signInManager,
            UserManager<ApplicationUserEntity> accountManager)
            : base(db, accountManager, signInManager)
        {
        }

        public async Task<List<ApplicationUserEntity>> GetByAdmin(Guid adminId)
        {
            string sql = $@"SELECT * 
                            FROM ApplicationUsers 
                            WHERE AdminId = @AdminId;";

            var parameters = new { AdminId = adminId };

            return await _db.GetAllData<ApplicationUserEntity, dynamic>(sql, parameters);
        }

        public Task<JwtModelDto> GetTenantJwtConfiguration(ApplicationUserEntity tenantEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}
