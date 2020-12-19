using AuthenticationServer.Common.Models.DTOs;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Logic.Managers
{
    public interface ITenantAccountManager
    {
        Task CreateTenantAccountAsync(TenantDto tenant);
    }
}
