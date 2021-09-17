using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Models.DTOs.Account
{
    public class AdminAccountDto: AbstractAccountDto
    {


        public List<TenantAccountDto> Tenants { get; set; }
    }
}
