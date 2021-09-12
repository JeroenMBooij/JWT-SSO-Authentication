using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Logic.Factories;
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

        public AccountController(IAccountServiceFactory accountServiceFactory)
        {
            _accountServiceFactory = accountServiceFactory;
        }



        /// <summary>
        /// Change your account's password
        /// </summary>
        /// <param name="newCredentials"></param>
        [HttpPost]
        [Route("change-password")]
        public async Task ChangePassword([FromBody] NewCredentials newCredentials)
        {
            IAccountService accountService = await _accountServiceFactory.CreateAccountService(newCredentials.Email);

            await accountService.ChangePassword(newCredentials);
        }

        /// <summary>
        /// Send a password reset email to your email to recover your account
        /// </summary>
        /// <param name="email"></param>
        [HttpPost]
        [Route("reset-password")]
        public async Task ResetPassword([FromBody] string email)
        {
            IAccountService accountService = await _accountServiceFactory.CreateAccountService(email);

            await accountService.ResetPassword(email);
        }

        /// <summary>
        /// Send a password recover email to your email to recover your account
        /// </summary>
        /// <param name="resetPasswordModel"></param>
        [HttpPost]
        [Route("recover-password")]
        public async Task RecoverPassword([FromForm] ResetPasswordModel resetPasswordModel)
        {
            IAccountService accountService = await _accountServiceFactory.CreateAccountService(resetPasswordModel.Email);

            await accountService.RecoverPassword(resetPasswordModel);
        }

        [HttpGet]
        [Route("VerifyEmail/{code}")]
        public async Task<string> VerifyEmail(string code)
        {
            IAccountService accountService = await _accountServiceFactory.CreateAccountService(Guid.Parse(code));

            await accountService.VerifyEmail(code);

            return "verfied";
            //return new ContentResult
            //{
            //    ContentType = "text/html",
            //    Content = "<div>Hello World</div>"
            //};
        }
    }
}
