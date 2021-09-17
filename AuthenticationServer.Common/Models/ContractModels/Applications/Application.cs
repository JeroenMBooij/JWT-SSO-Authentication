

using AuthenticationServer.Common.Models.ContractModels.Token;
using System;
using System.Collections.Generic;

namespace AuthenticationServer.Common.Models.ContractModels.Applications
{
    public class Application
    {
        public string Name { get; set; }
        public List<DomainName> Domains { get; set; }
        public string IconUUID { get; set; }
        public List<JwtTenantConfig> JwtTenantConfigurations { get; set; }

    }
}