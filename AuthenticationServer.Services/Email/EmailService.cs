using AuthenticationServer.Common.Generated;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IEmailClient _emailClient;
        private readonly IConfiguration _config;

        public EmailService(IEmailClient emailClient, IConfiguration config)
        {
            _emailClient = emailClient;
            _config = config;
        }

        public async Task SendRecoverPasswordEmail(string recipient, string passwordRecoverToken)
        {
            string subject = "Recover Password";
            string verificationLink = $@"   <p> Enter your new password: </p>
                                            <form action=""{_config["BaseUrls:AuthenticationServer"]}TenantAccount/recover-password"" method=""post""
                                                enctype=""application/x-www-form-urlencoded"">
                                                    <input type = ""text"" name=""NewPassword"">
                                                    <input type = ""hidden"" name=""ResetToken"" value=""{passwordRecoverToken}"">
                                                    <input type = ""hidden"" name=""Email"" value =""{recipient}"">
                                             
                                                    <br><br>
                                                    <input type = ""submit"" value=""Submit"" >
                                            </ form > ";

            var recipients = new List<string>() { recipient };
            var message = new Message()
            {
                //TODO enable configuration
                    Sender = "jmbooij.a@gmail.com",
                    AppKey = _config["APPKEY"],
                //
                Recipients = recipients,
                Subject = subject,
                Content = verificationLink
            };

            await _emailClient.HtmlAsync(message);
        }

        public async Task SendVerificationEmail(string recipient, Guid code)
        {
            string subject = "Verify Email";
            string verificationLink = $@"<a href='{_config["BaseUrls:AuthenticationServer"]}Email/VerifyEmail/{code}'> Verification Link </a>";

            var recipients = new List<string>() { recipient };
            var message = new Message()
            {
                Recipients = recipients,
                Subject = subject,
                Content = verificationLink
            };

            await _emailClient.HtmlAsync(message);
        }
    }
}
