using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantRegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public TenantRegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        public async Task<string> RegisterUser(Tenant tenant)
        {
            return await _registerService.RegisterTenant(tenant);
        }
    }
}
