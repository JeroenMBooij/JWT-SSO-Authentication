using AuthenticationServer.Common.Models.ContractModels.Token;
using AuthenticationServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface IAccountRepository: ICRUDRepository<ApplicationUserEntity>
    {
        Task AddRoleToAccount(ApplicationUserEntity account, string role);
        Task<IdentityResult> ChangePassword(string email, string oldPassword, string newPassword);
        Task<ApplicationUserEntity> GetAccountByEmail(string email);
        Task<Guid?> GetAdminIdByEmail(string email);
        Task<DateTimeOffset?> GetLockoutTime(string email);
        Task<IdentityResult> RecoverPassword(string email, string resetToken, string newPassword);
        Task<string> ResetPassword(string email);
        Task SetVerified(Guid id);
        Task<SignInResult> SignInAccount(ApplicationUserEntity tenantEntity, string password);
        Task<SignInResult> SignInAccount(string email, string password);
        Task RegisterTicket(Guid id, Ticket ticket);
        Task<Ticket> GetTicket(Guid userId);
        Task RegisterJWT(Guid accountId, Guid? applicationId, string registeredJWT);
    }
}