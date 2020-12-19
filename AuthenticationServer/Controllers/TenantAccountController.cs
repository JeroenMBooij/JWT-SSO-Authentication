using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantAccountController : ControllerBase
    {
        private readonly ITenantAccountService _tenantAccountService;

        public TenantAccountController(ITenantAccountService tenantAccountService)
        {
            _tenantAccountService = tenantAccountService;
        }

        [HttpPost]
        public async Task<string> RegisterTenant([FromBody]Tenant tenant)
        {
            return await _tenantAccountService.RegisterTenantAsync(tenant);
        }
    }
}
