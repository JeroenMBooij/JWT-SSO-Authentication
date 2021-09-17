using AuthenticationServer.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Models.ContractModels
{
    public class SecurityDescription
    {
        public SecurityAlgorithm Name { get; set; }
        public string Schema { get; set; }
        public EncryptionType Type {get; set;}
    }
}
