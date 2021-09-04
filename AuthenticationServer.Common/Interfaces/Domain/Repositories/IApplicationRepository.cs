using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface IApplicationRepository
    {
        Task Insert(ApplicationEntity domain);
        Task<ApplicationEntity> Get(Guid id);
        Task<List<ApplicationEntity>> GetAll();
        Task Update(ApplicationEntity domain);
        Task Delete(ApplicationEntity domain);
        Task<ApplicationEntity> GetDomainFromUrl(string url);
        Task<ApplicationEntity> GetDomainFromName(string name);
        Task<List<ApplicationEntity>> GetDomainsFromAdminId(string adminId);
        Task AddTenantToAdmin(Guid id, string adminId);
    }
}
