using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Logic.Generated;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Managers
{
    public class EmailManager : IEmailManager
    {
        private readonly IEmailClient _emailClient;
        private readonly IConfiguration _config;

        public EmailManager(IEmailClient emailClient, IConfiguration config)
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
                Recipients = recipients,
                Subject = subject,
                Content = verificationLink
            };

            await _emailClient.SendHtmlEmailAsync(message);
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

            await _emailClient.SendHtmlEmailAsync(message);
        }
    }
}
