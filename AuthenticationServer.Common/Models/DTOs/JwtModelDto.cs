using AuthenticationServer.Common.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class JwtModelDto
    {
        public string SecretKey { get; set; }
        public SecurityAlgorithm SecurityAlgorithm { get; set; }
        public double? ExpireMinutes { get; set; }
        public Claim[] Claims { get; set; }

    }
}
