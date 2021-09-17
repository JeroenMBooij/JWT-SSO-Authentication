using System;

namespace AuthenticationServer.Common.Models.ContractModels.Account
{
    public class AccountRegistration : AccountData
    {
        public string Password { get; set; }
        public string AuthenticationRole { get; set; }
        public string ApplicationId { get; set; }
        public string AdminId { get; set; }
    }
}
