using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public UserRegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        public async Task<string> RegisterUser(User user)
        {
            return await _registerService.RegisterUser(user);
        }
    }
}
