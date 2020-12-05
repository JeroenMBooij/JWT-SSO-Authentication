using AuthenticationServer.Logic.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace AuthenticationServer.Logic.Models
{
    public class JWTContainerModel : IAuthContainerModel
    {
        public string SecretKey { get; set; }
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; } 
        public Claim[] Claims { get; set; }

    }
}
