using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class JwtConfigurationDto
    {
        public string SecretKey { get; set; }
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public long ExpireHours { get; set; }
        public Claim[] Claims { get; set; }

    }
}
