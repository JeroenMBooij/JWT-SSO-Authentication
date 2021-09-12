using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Token;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class JwtTenantConfigDto : JwtConfig
    {
        public Guid Id { get; set; }
        public List<ClaimConfig> Claims { get; set; }
        public bool LockoutEnabled { get; set; }
        public ApplicationDto Application { get; set; }
    }
}
