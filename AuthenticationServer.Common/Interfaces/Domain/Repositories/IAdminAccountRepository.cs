using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface IAdminAccountRepository: IAccountRepository
    {
        Task AddTenantToAdmin(Guid id, string adminId);
    }
}
