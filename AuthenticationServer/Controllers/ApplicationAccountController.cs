using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.ContractModels.Applications;
using AuthenticationServer.Web.Middleware.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationAccountController : ControllerBase
    {
        private readonly IAdminAccountService _applicationService;

        public ApplicationAccountController(IAdminAccountService applicationService)
        {
            _applicationService = applicationService;
        }

        /// <summary>
        /// Login with your application Account
        /// </summary>
        /// <return>
        /// jwt token
        /// </return>
        /// <param name="credentials"></param>
        [HttpPost]
        [Route("Login")]
        public async Task<string> Login([FromBody] Credentials credentials)
        {
            return await _applicationService.LoginAsync(credentials);
        }

        /// <summary>
        /// Register an application to manage your users
        /// </summary>
        /// <param name="applicationAccount"></param>
        /// <returns>validation message</returns>
        [HttpPost]
        [Route("Register")]
        [SuccessStatusCode(StatusCodes.Status201Created)]
        public async Task<string> Register([FromBody] AdminAccount applicationAccount)
        {
            return await _applicationService.RegisterAsync(applicationAccount);
        }

        /// <summary>
        /// Update your application
        /// </summary>
        /// <remarks>
        /// Name is required for identifleave the values
        /// </remarks>
        /// <param name="application"></param>
        /// <returns>validation message</returns>
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ApplicationManager")]
        [SuccessStatusCode(StatusCodes.Status201Created)]
        public async Task Update([FromBody] ApplicationWithId application)
        {
            //TODO validate if jwt owns application
            await _applicationService.UpdateApplicationAsync(application);
        }

    }
}
