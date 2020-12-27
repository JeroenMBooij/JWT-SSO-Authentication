using AuthenticationServer.Common.Models.DTOs;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Logic.Managers
{
    public interface ITenantAccountManager
    {
        Task<TenantDto> CreateTenantAccountAsync(TenantDto tenant);
    }
}
