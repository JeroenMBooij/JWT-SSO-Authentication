using AuthenticationServer.Common.Models.ContractModels.Account;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface ITenantStatisticsService
    {
        Task<PaginatedAccounts> GetTenantsPaginated(string token, int startIndex, int endIndex);
    }
}