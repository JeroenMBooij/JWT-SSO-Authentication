using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Logic.Managers
{
    public interface IJwtManager
    {
        bool IsTokenValid(string token);
        bool IsTokenSignatureValid(string token);
        string GenerateToken(JwtConfigurationDto model);
        IEnumerable<Claim> GetTokenClaims(string token);
        string GetUserId(string token);
        IEnumerable<Claim> GetTenantClaims(AccountDto tenantDto);
        Task<AccountDto> GetApplicationUserDto(string token);
        JToken DeserializeToken(string token);
        JwtSecurityToken HandleToken(string token);

    }
}
