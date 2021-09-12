using AuthenticationServer.Common.Models.ContractModels.Applications;
using System.Collections.Generic;

namespace AuthenticationServer.Common.Models.ContractModels.Account
{
    public class AdminAccount : AbstractAccount
    {
        public List<Application> Assets { get; set; }
        public string Token { get; set; }
    }
}
