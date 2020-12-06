using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;

namespace AuthenticationServer.Common.Models.ResponseModels
{
    public class JwtConfiguration
    {
        public string SecretKey { get; set; }
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; }
        public Claim[] Claims { get; set; }
        public DateTimeOffset Nbf { get; set; }
        public DateTimeOffset Exp { get; set; }

    }
}
