using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationServer.Common.Models.ContractModels.Account
{
    public class ApplicationAccount: AbstractAccount
    {
        public DomainName Asset { get; set; }
        public string Token { get; set; }
    }
}
