using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Controllers
{
    [Route("api/tenant", Name = "Tenant Statistics")]
    [ApiController]
    public class TenantStatisticsController : ControllerBase
    {
        private readonly ITenantStatisticsService _tenantService;

        public TenantStatisticsController(ITenantStatisticsService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet("start/{startIndex}/end/{endIndex}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<PaginatedAccounts> GetTenants(int startIndex, int endIndex)
        {
            string token = Request.Headers["Authorization"].ToString();

            return await _tenantService.GetTenantsPaginated(token, startIndex, endIndex);
        }
    }
}
