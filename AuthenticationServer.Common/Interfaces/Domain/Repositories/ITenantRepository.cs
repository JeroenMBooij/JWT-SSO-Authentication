using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
            
namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface ITenantRepository
    {
        Task Insert(ApplicationUserEntity tenantEntity, string password);
        Task<ApplicationUserEntity> Get(Guid Id);
        Task<List<ApplicationUserEntity>> GetAll();
        Task Update(ApplicationUserEntity tenantEntity);
        Task Delete(ApplicationUserEntity tenantEntity);

        Task<ApplicationUserEntity> GetTenantByEmail(string email);
        Task<SignInResult> SignInTenant(string email, string password);
        Task<SignInResult> SignInTenant(ApplicationUserEntity tenantEntity, string password);
        Task<JwtConfigurationDto> GetTenantJwtConfiguration(ApplicationUserEntity tenantDTO);
        Task SetVerified(Guid id);
        Task<DateTimeOffset?> GetTenantLockoutTime(string email);
        Task<bool> TenantOwnsDomain(string email, string url);
        Task<IdentityResult> ChangePassword(string email, string oldPassword, string newPassword);
        Task<string> ResetPassword(string email);
        Task<IdentityResult> RecoverPassword(string email, string resetToken, string newPassword);
        Task AddRoleToAccount(ApplicationUserEntity account, string role);
    }
}
