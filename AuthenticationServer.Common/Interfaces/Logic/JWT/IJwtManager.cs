using AuthenticationServer.Common.Models.ResponseModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthenticationServer.Common.Interfaces.Logic.JWT
{
    public interface IJwtManager
    {
        string SecretKey { get; set; }

        bool IsTokenValid(string token);
        string GenerateToken(JwtConfiguration model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
