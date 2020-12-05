using AuthenticationServer.Models.ViewModels;
using AuthenticationServer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        public string RegisterUser(UserVM user)
        {
            return _registerService.RegisterUser(user);
        }
    }
}
