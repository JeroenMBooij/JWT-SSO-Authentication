using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.ContractModels.Token;
using AuthenticationServer.Logic.Factories;
using AuthenticationServer.Web.Middleware.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServiceFactory _accountServiceFactory;
        private IAccountService _accountService;

        public AccountController(IAccountServiceFactory accountServiceFactory)
        {
            _accountServiceFactory = accountServiceFactory;
        }



        /// <summary>
        /// Login with your Tenant Account
        /// </summary>
        /// <remarks>
        /// **ApplicationId:** optional parameter: Used to identify the application a Tenant is using. If null the hostname of the request will be used to identify the application
        /// </remarks>
        /// <return>
        /// jwt token
        /// </return>
        /// <param name="credentials"></param>
        [HttpPost]
        [Route("Login")]
        public async Task<Ticket> Login([FromBody] Credentials credentials)
        {
            _accountService = await _accountServiceFactory.CreateAccountService(credentials.Email);

            if (_accountService == null)
                throw new AuthenticationApiException("login", "Invalid Credentials provided");

            string hostname = Request.GetDomainUrl();

            return await _accountService.LoginAsync(credentials, hostname);
        }



        /// <summary>
        /// Register an application to manage your users
        /// </summary>
        /// <remarks>
        /// **AuthenticationRole:** options = ["Admin", "Tenant"]<br/><br/>
        /// **ConfigData:** Custom tenant atrributes and values to store inside the database as json <br/><br/>
        /// **ApplicationId** Optional parameter to specify the application the user registered on <br/><br/>
        /// **AdminId** Only required for Tenant accounts <br/><br/>
        /// </remarks>
        /// <param name="accountData"></param>
        /// <returns>Registration message</returns>
        [HttpPost]
        [Route("Register")]
        [SuccessStatusCode(StatusCodes.Status201Created)]
        public async Task<string> Register([FromBody] AccountRegistration accountData)
        {
            _accountService = _accountServiceFactory.CreateAccountService(Enum.Parse<AccountRole>(accountData.AuthenticationRole));

            return await _accountService.RegisterAsync(accountData);
        }


        /// <summary>
        /// Change an account's password
        /// </summary>
        /// <param name="newCredentials"></param>
        [HttpPost]
        [Route("change-password")]
        public async Task ChangePassword([FromBody] NewCredentials newCredentials)
        {
            _accountService = await _accountServiceFactory.CreateAccountService(newCredentials.Email);

            await _accountService.ChangePassword(newCredentials);
        }

        /// <summary>
        /// Send a password reset email to your email to recover an account
        /// </summary>
        /// <param name="email"></param>
        [HttpPost]
        [Route("reset-password")]
        public async Task ResetPassword([FromBody] string email)
        {
            _accountService = await _accountServiceFactory.CreateAccountService(email);

            await _accountService.ResetPassword(email);
        }

        /// <summary>
        /// Send a password recover email to your email to recover an account
        /// </summary>
        /// <param name="resetPasswordModel"></param>
        [HttpPost]
        [Route("recover-password")]
        public async Task RecoverPassword([FromForm] ResetPasswordModel resetPasswordModel)
        {
            _accountService = await _accountServiceFactory.CreateAccountService(resetPasswordModel.Email);

            await _accountService.RecoverPassword(resetPasswordModel);
        }

        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<string> VerifyEmail([FromBody]string code)
        {
            _accountService = await _accountServiceFactory.CreateAccountService(Guid.Parse(code));

            await _accountService.VerifyEmail(code);

            return "verfied";
            //return new ContentResult
            //{
            //    ContentType = "text/html",
            //    Content = "<div>Hello World</div>"
            //};
        }
    }
}
