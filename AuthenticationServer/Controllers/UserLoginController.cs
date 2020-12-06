using AuthenticationServer.Common.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public UserLoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }


    }
}
