using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Models.DTOs.Account
{
    public class TenantAccountDto: AbstractAccountDto
    {
        public List<string> Roles { get; set; }
        public string AdminToken { get; set; }
        public Guid AdminId { get; set; }
        public AdminAccountDto Admin { get; set; }

    }
}
