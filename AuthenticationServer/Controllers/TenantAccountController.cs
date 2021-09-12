using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Extentions;
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
    public class TenantAccountController : ControllerBase
    {
        private readonly ITenantAccountService _tenantAccountService;

        public TenantAccountController(ITenantAccountService tenantAccountService)
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
        [HttpPost]
        [Route("Register")]
        [SuccessStatusCode(StatusCodes.Status201Created)]
        public async Task<string> Register([FromBody] AccountRegistration tenant)
        {
            string token = Request.Headers["Authorization"].ToString();
            
            if(string.IsNullOrEmpty(token) == false)
                return await _tenantAccountService.RegisterWithTokenAsync(tenant, token);


            string hostname = Request.GetDomainUrl();

            return await _tenantAccountService.RegisterWithHostnameAsync(tenant, hostname);
        }

        /// <summary>
        /// Login with your Tenant Account
        /// </summary>
        /// <param name="applicationId">specify which jwt token config needs to be used</param>
        /// <return>
        /// jwt token
        /// </return>
        /// <param name="credentials"></param>
        [HttpPost]
        [Route("Login/{applicationId}")]
        public async Task<string> Login(string applicationId = "", [FromBody] Credentials credentials = null)
        {
            if (credentials == null)
                throw new AuthenticationApiException("login", "No Credentials provided");

            if (string.IsNullOrEmpty(applicationId) == false)
                return await _tenantAccountService.LoginAsync(credentials, applicationId);


            string hostname = Request.GetDomainUrl();

            return await _tenantAccountService.LoginAsync(credentials, hostname);
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
