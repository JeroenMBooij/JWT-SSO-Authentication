using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class JwtConfigurationDto
    {
        public string SecretKey { get; set; }
        public string SecurityAlgorithm { get; set; }
        public double ExpireMinutes { get; set; }
        public List<Claim> ConfiguredClaims { get; set; }
        public Claim[] Claims { get; set; }

    }
}
