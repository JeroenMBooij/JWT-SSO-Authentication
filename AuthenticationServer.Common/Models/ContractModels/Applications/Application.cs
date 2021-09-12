

using AuthenticationServer.Common.Models.ContractModels.Token;
using System.Collections.Generic;

namespace AuthenticationServer.Common.Models.ContractModels.Applications
{
    public class Application
    {
        public string Name { get; set; }
        public List<DomainName> Domains { get; set; }
        public string Logo { get; set; }
        public List<JwtTenantConfig> JwtTenantConfigurations { get; set; }

    }
}