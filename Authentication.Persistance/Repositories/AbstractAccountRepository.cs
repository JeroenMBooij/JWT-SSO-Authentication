using AuthenticationServer.Common.Constants.StoredProcedures;
using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Domain.Entities;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Persistance.Repositories
{
    public abstract class AbstractAccountRepository
    {
        protected readonly IMainSqlDataAccess _db;
        protected readonly UserManager<ApplicationUserEntity> _tenantAccountManager;
        private readonly SignInManager<ApplicationUserEntity> _signInManager;

        public AbstractAccountRepository(IMainSqlDataAccess db, UserManager<ApplicationUserEntity> tenantAccountManager,
            SignInManager<ApplicationUserEntity> signInManager)
        {
            _db = db;
            _tenantAccountManager = tenantAccountManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUserEntity> GetAccountByEmail(string email)
        {
            ApplicationUserEntity applicationUserEntity = await _tenantAccountManager.FindByEmailAsync(email);
            if (applicationUserEntity == null)
                throw new AuthenticationApiException("Tenant Account", $"No account was found for {email}");

            string sql = $"SELECT * FROM dbo.AspNetRoles where ApplicationUserEntityId = @Id";
            var parameters = new { Id = applicationUserEntity.Id.ToString() };

            applicationUserEntity.Roles = await _db.GetData<List<RoleEntity>, dynamic>(sql, parameters);

            return applicationUserEntity;
        }

        public async Task AddRoleToAccount(ApplicationUserEntity account, string role)
        {
            await _tenantAccountManager.AddToRoleAsync(account, role);
        }

        public async Task<SignInResult> SignInAccount(string email, string password)
        {
            return await _signInManager.PasswordSignInAsync(email, password, false, true);
        }

        public async Task<SignInResult> SignInAccount(ApplicationUserEntity tenantEntity, string password)
        {
            return await _signInManager.PasswordSignInAsync(tenantEntity, password, false, true);
        }


        public virtual async Task Insert(ApplicationUserEntity tenantEntity, string password = "")
        {
            try
            {
                await GetAccountByEmail(tenantEntity.Email);
                throw new AuthenticationApiException("Tenant Account", "There already is an account with that email.");
            }
            catch (AuthenticationApiException) { }
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("password is required");

            var parametersToStoredProcedure = new Dictionary<DynamicParameters, string>();
            AddLanguageToAccount(parametersToStoredProcedure, tenantEntity.Id, tenantEntity.Languages.ToList());
            tenantEntity.Languages.Clear();

            IdentityResult result = await _tenantAccountManager.CreateAsync(tenantEntity, password);

            if (result.Succeeded)
                await _db.ExecuteStoredProcedures(parametersToStoredProcedure);
            else
                throw new AuthenticationApiException(result.Errors.ToErrorModel());
        }

        public virtual async Task<ApplicationUserEntity> Get(Guid id)
        {
            string sql = $"SELECT * FROM dbo.ApplicationUsers where Id = @Id";
            var parameters = new { Id = id.ToString() };

            return await _db.GetData<ApplicationUserEntity, dynamic>(sql, parameters);
        }


        public async Task SetVerified(Guid id)
        {
            string sql = $"UPDATE dbo.ApplicationUsers SET EmailConfirmed = '1' where Id = @Id";
            var parameters = new { Id = id.ToString() };

            await _db.SaveData<ApplicationUserEntity, dynamic>(sql, parameters);
        }
        public async Task<DateTimeOffset?> GetLockoutTime(string email)
        {
            var tenantEntity = await GetAccountByEmail(email);
            return await _tenantAccountManager.GetLockoutEndDateAsync(tenantEntity);
        }

        public async Task<IdentityResult> ChangePassword(string email, string oldPassword, string newPassword)
        {
            ApplicationUserEntity tenantEntity = await GetAccountByEmail(email);
            IdentityResult result = await _tenantAccountManager.ChangePasswordAsync(tenantEntity, oldPassword, newPassword);

            return result;
        }

        public async Task<string> ResetPassword(string email)
        {
            ApplicationUserEntity tenantEntity = await GetAccountByEmail(email);

            string passwordResetToken = await _tenantAccountManager.GeneratePasswordResetTokenAsync(tenantEntity);

            return passwordResetToken;
        }

        public async Task<IdentityResult> RecoverPassword(string email, string resetToken, string newPassword)
        {
            ApplicationUserEntity tenantEntity = await GetAccountByEmail(email);

            IdentityResult result = await _tenantAccountManager.ResetPasswordAsync(tenantEntity, resetToken, newPassword);

            return result;
        }

        public virtual Task<List<ApplicationUserEntity>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public virtual Task Update(Guid id, ApplicationUserEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task Delete(ApplicationUserEntity tenantEntity)
        {
            throw new System.NotImplementedException();
        }







        private void AddLanguageToAccount(Dictionary<DynamicParameters, string> parametersToStoredProcedure, Guid accountId, List<LanguageEntity> languages)
        {
            foreach (LanguageEntity languageEntity in languages)
            {
                var tenantLanguageParameters = new DynamicParameters();
                tenantLanguageParameters.Add("LanguageId", languageEntity.Id);
                tenantLanguageParameters.Add("UserId", accountId);

                parametersToStoredProcedure.Add(tenantLanguageParameters, SPName.InsertAccountLanguage);
            }
        }

    }
}
