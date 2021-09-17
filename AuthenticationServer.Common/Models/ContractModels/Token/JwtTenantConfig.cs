using AuthenticationServer.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.Common.Models.ContractModels.Token
{
    public class JwtTenantConfig
    {
        [Required]
        public string SecretKey { get; set; }

        [Required]
        public List<ClaimConfig> Claims { get; set; }

        public double? ExpireMinutes { get; set; }

        public double? RefreshExpireMinutes { get; set; }

        [Required]
        public bool LockoutEnabled { get; set; }

        [Required]
        public bool ValidateIssuer { get; set; }

        public string Issuer { get; set; }

        [Required]
        public string Algorithm { get; set; }
    }
}
