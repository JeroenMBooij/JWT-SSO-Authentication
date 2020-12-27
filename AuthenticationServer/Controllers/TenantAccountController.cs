using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TenantAccountController : ControllerBase
    {
        private readonly ITenantAccountService _tenantAccountService;

        public TenantAccountController(ITenantAccountService tenantAccountService)
        {
            _tenantAccountService = tenantAccountService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<string> RegisterTenant([FromBody]Tenant tenant)
        {
            return await _tenantAccountService.RegisterTenantAsync(tenant);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<string> LoginTenant([FromBody] Credentials credentials)
        {
            return await _tenantAccountService.LoginTenantAsync(credentials);
        }
    }
}
