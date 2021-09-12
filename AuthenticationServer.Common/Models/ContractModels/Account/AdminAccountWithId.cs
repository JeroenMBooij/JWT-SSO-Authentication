using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Models.ContractModels.Account
{
    public class AdminAccountWithId : AdminAccount
    {
        public Guid Id { get; set; }
    }
}
