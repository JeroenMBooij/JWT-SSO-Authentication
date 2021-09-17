using System.Collections.Generic;

namespace AuthenticationServer.Common.Models.ContractModels.Account
{
    public class AccountData
    {
        public string Email { get; set; }
        //TODO unique constraints on configData columns
        public string ConfigData { get; set; }

    }
}
