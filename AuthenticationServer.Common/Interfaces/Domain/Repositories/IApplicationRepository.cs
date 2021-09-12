using AuthenticationServer.Common.Enums;
using AuthenticationServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Domain.Repositories
{
    public interface IApplicationRepository: IRepository<ApplicationEntity>
    {
        Task<AccountRole> GetAccountRoleFromEmail(string email);
        Task<ApplicationEntity> GetApplicationFromHostname(string url);
        Task<ApplicationEntity> GetApplicationFromName(string name);
        Task<List<ApplicationEntity>> GetApplicationsFromAdminId(Guid adminId);
        Task<AccountRole> GetAccountRoleFromId(Guid id);
    }
}
