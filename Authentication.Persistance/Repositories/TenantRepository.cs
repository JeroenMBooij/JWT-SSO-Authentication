using AuthenticationServer.Common.Constants.StoredProcedures;
using AuthenticationServer.Common.Exceptions;
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
    public class TenantRepository : AbstractRepository, ITenantRepository
    {
        private readonly IMainSqlDataAccess _db;
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUserEntity> _signInManager;
        private readonly UserManager<ApplicationUserEntity> _tenantAccountManager;

        public TenantRepository(IMainSqlDataAccess db, IConfiguration config,
            SignInManager<ApplicationUserEntity> signInManager, UserManager<ApplicationUserEntity> tenantAccountManager)
        {
            _db = db;
            _config = config;
            _signInManager = signInManager;
            _tenantAccountManager = tenantAccountManager;
        }

        public Task Delete(ApplicationUserEntity tenantEntity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ApplicationUserEntity> Get(Guid id)
        {
            string sql = $"SELECT * FROM dbo.ApplicationUsers where Id = @Id";
            var parameters = new { Id = id.ToString() };

            return await _db.GetData<ApplicationUserEntity, dynamic>(sql, parameters);
        }

        public Task<List<ApplicationUserEntity>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<JwtConfigurationDto> GetTenantJwtConfiguration(ApplicationUserEntity tenantEntity)
        {
            throw new System.NotImplementedException();
        }

        public async Task Insert(ApplicationUserEntity tenantEntity, string password)
        {
            try
            {
                await GetTenantByEmail(tenantEntity.Email);
                throw new AuthenticationApiException("Tenant Account", "There already is an account with that email.");
            }
            catch (AuthenticationApiException){}
                

            var parametersToStoredProcedure = new Dictionary<DynamicParameters, string>();

            foreach (LanguageEntity languageEntity in tenantEntity.Languages)
            {
                var tenantLanguageParameters = new DynamicParameters();
                tenantLanguageParameters.Add("LanguageId", languageEntity.Id);
                tenantLanguageParameters.Add("UserId", tenantEntity.Id);

                parametersToStoredProcedure.Add(tenantLanguageParameters, SPName.InsertAccountLanguage);
            }

            tenantEntity.Languages.Clear();
            tenantEntity.LockoutEnabled = bool.Parse(_config["TenantIdentityConfiguration:LockOut"]);

            await _tenantAccountManager.CreateAsync(tenantEntity, password);

            await _db.ExecuteStoredProcedures(parametersToStoredProcedure);
        }

        public async Task SetVerified(Guid id)
        {
            string sql = $"UPDATE dbo.ApplicationUsers SET EmailConfirmed = '1' where Id = @Id";
            var parameters = new { Id = id.ToString() };

            await _db.SaveData<ApplicationUserEntity, dynamic>(sql, parameters);
        }

        public async Task<SignInResult> SignInTenant(string email, string password)
        {
            return await _signInManager.PasswordSignInAsync(email, password, false, true);
        }

        public async Task<SignInResult> SignInTenant(ApplicationUserEntity tenantEntity, string password)
        {
            return await _signInManager.PasswordSignInAsync(tenantEntity, password, false, true);
        }

        public Task Update(ApplicationUserEntity tenantEntity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ApplicationUserEntity> GetTenantByEmail(string email)
        {
            ApplicationUserEntity tenantEntity = await _tenantAccountManager.FindByEmailAsync(email);
            if (tenantEntity == null)
                throw new AuthenticationApiException("Tenant Account", $"No account was found for {email}");

            string sql = $"SELECT * FROM dbo.AspNetRoles where ApplicationUserEntityId = @Id";
            var parameters = new { Id = tenantEntity.Id.ToString() };

            tenantEntity.Roles = await _db.GetData<List<RoleEntity>, dynamic>(sql, parameters);

            return tenantEntity;
        }

        public async Task<DateTimeOffset?> GetTenantLockoutTime(string email)
        {
            var tenantEntity = await GetTenantByEmail(email);
            return await _tenantAccountManager.GetLockoutEndDateAsync(tenantEntity);
        }

        public async Task<bool> TenantOwnsDomain(string email, string url)
        {
            ApplicationUserEntity tenantEntity = await GetTenantByEmail(email);

            return tenantEntity.Assets.Where(domain => domain.Url.Equals(url)) != null;
        }

        public async Task<IdentityResult> ChangePassword(string email, string oldPassword, string newPassword)
        {
            ApplicationUserEntity tenantEntity = await GetTenantByEmail(email);
            IdentityResult result = await _tenantAccountManager.ChangePasswordAsync(tenantEntity, oldPassword, newPassword);

            return result;
        }

        public async Task<string> ResetPassword(string email)
        {
            ApplicationUserEntity tenantEntity = await GetTenantByEmail(email);

            string passwordResetToken = await _tenantAccountManager.GeneratePasswordResetTokenAsync(tenantEntity);

            return passwordResetToken;
        }

        public async Task<IdentityResult> RecoverPassword(string email, string resetToken, string newPassword)
        {
            ApplicationUserEntity tenantEntity = await GetTenantByEmail(email);

            IdentityResult result = await _tenantAccountManager.ResetPasswordAsync(tenantEntity, resetToken, newPassword);

            return result;
        }

        public async Task AddRoleToAccount(ApplicationUserEntity account, string role)
        {
            await _tenantAccountManager.AddToRoleAsync(account, role);
        }
    }
}
