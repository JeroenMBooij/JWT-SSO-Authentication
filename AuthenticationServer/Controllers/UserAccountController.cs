using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;

        public UserAccountController(IUserAccountService registerService)
        {
            _userAccountService = registerService;
        }

        [HttpPost]
        public async Task<string> RegisterUser([FromBody]User user)
        {
            return await _userAccountService.RegisterUserAsync(user, Request.Host.Value);
        }
    }
}
