using AuthenticationServer.Logic.Models.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthenticationServer.Logic.Managers.Interfaces
{
    interface IAuthManager
    {
        string SecretKey { get; set; }

        bool IsTokenValid(string token);
        string GenerateToken(IAuthContainerModel model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
