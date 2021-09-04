using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Web.Middleware.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _tenantAccountService;

        public AccountController(IAccountService tenantAccountService)
        {
            _tenantAccountService = tenantAccountService;
        }

        [HttpGet]
        [Route("test")]
        public async Task<string> TestTenant()
        {
            Debug.WriteLine("begin");
            await Task.Delay(5000);
            Debug.WriteLine("test");
            return "test";
        }

        /// <summary>
        /// Create an account
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns>An email validation message</returns>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ApplicationManager")]
        [HttpPost]
        [Route("Register")]
        [SuccessStatusCode(StatusCodes.Status201Created)]
        public async Task<string> Register([FromBody] AccountRegistration tenant)
        {
            string token = Request.Headers["Authorization"].ToString();
            return await _tenantAccountService.RegisterTenantAsync(tenant, token);
        }

        /// <summary>
        /// Login with your Tenant Account
        /// </summary>
        /// <remarks>
        /// On succesful login we will add a token to your cookies with a 'tenant-authorization-token' key,
        /// so we will be able to retrieve the token from subsequent requests
        /// </remarks>
        /// <param name="credentials"></param>
        [HttpPost]
        [Route("Login")]
        public async Task<string> Login([FromBody] Credentials credentials)
        {
            return await _tenantAccountService.LoginTenantAsync(credentials);
        }

        /// <summary>
        /// Change your Tenant account password
        /// </summary>
        /// <param name="newCredentials"></param>
        [HttpPost]
        [Route("change-password")]
        public async Task ChangePassword([FromBody] NewCredentials newCredentials)
        {
            await _tenantAccountService.ChangePassword(newCredentials);
        }

        /// <summary>
        /// Send a password reset email to your email to recover your account
        /// </summary>
        /// <param name="email"></param>
        [HttpPost]
        [Route("reset-password")]
        public async Task ResetPassword([FromBody] string email)
        {
            await _tenantAccountService.ResetPassword(email);
        }

        /// <summary>
        /// Send a password recover email to your email to recover your account
        /// </summary>
        /// <param name="resetPasswordModel"></param>
        [HttpPost]
        [Route("recover-password")]
        public async Task RecoverPassword([FromForm] ResetPasswordModel resetPasswordModel)
        {
            await _tenantAccountService.RecoverPassword(resetPasswordModel);
        }

        /*/// <summary>
        /// Send a password reset email to your email to recover your tenant account
        /// </summary>
        /// <param name="resetPasswordModel"></param>
        [HttpPost]
        [Route("recover-password")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> RecoverPassword([FromForm] ResetPasswordModel resetPasswordModel)
        {
            await _tenantAccountService.RecoverPassword(resetPasswordModel);

            return "TODO: make a pretty landing page for password changed, but for now: Password Changed Successfully";
        }*/

        [HttpPost]
        [Route("AuthenticationState")]
        public void AuthenticationState([FromBody] string token)
        {
            if (!_tenantAccountService.IsTokenValid(token))
                throw new AuthenticationApiException("jwtToken", "Is invalid");
        }

    }
}
