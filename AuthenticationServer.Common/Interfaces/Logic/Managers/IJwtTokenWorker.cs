using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Logic.Managers
{
    public interface IJwtTokenWorker
    {
        bool IsTokenValid(JwtTenantConfigDto jwtTenantConfigDto, string token);
        bool IsTokenValid(string token);
        bool IsTokenSignatureValid(JwtTenantConfigDto jwtTenantConfigDto, string token);
        string GenerateToken(JwtModelDto model);
        IEnumerable<Claim> GetTokenClaimsWithValidation(JwtTenantConfigDto jwtTenantConfigDto, string token);
        IEnumerable<Claim> GetTokenClaims(string token);
        Guid GetUserId(string token);
        JToken DeserializeToken(string token);

    }
}
