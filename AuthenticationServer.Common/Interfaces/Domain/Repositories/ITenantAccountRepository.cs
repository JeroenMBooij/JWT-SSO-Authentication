using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface ITenantAccountRepository : IAccountRepository
    {
        Task<JwtModelDto> GetTenantJwtConfiguration(ApplicationUserEntity tenantDTO);
        Task<List<ApplicationUserEntity>> GetByAdmin(Guid adminId);
    }
}
