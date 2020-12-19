using AuthenticationServer.Common.Models.DTOs;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthenticationServer.Common.Interfaces.Logic.Managers
{
    public interface IJwtManager
    {
        bool IsTokenValid(string token);
        string GenerateToken(JwtConfigurationDto model);
        IEnumerable<Claim> GetTokenClaims(string token);
        public IEnumerable<Claim> GetTenantClaims(TenantDto tenantDto);
        public IEnumerable<Claim> GetuserClaims(UserDto userDto);


    }
}
