using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Managers
{
    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _config;
        private readonly ITenantRepository _accountRepository;
        private readonly IMapper _mapper;

        public JwtManager(IConfiguration config, ITenantRepository accountRepository, IMapper mapper)
        {
            _config = config;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
        

        public string GenerateToken(JwtConfigurationDto model)
        {
            if (model == null || model.Claims == null || model.Claims.Length == 0)
                throw new ArgumentException("Arguments to create tokens are not valid");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _config["JwtAuthentication:Issuer"],
                Subject = new ClaimsIdentity(model.Claims),
                Expires = DateTime.Now.AddHours(model.ExpireHours),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), model.SecurityAlgorithm)
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            string token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return token;
        }

        private List<Claim> GetAccountClaims(AccountDto account)
        {
            return new List<Claim>
            {
                new Claim(type: "id", account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.Email)
            };
        }

        public IEnumerable<Claim> GetTenantClaims(AccountDto tenantDto)
        {
            var accountClaims = GetAccountClaims(tenantDto);

            return accountClaims;
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return tokenValid.Claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty.");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

        public bool IsTokenSignatureValid(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty.");

            TokenValidationParameters tokenValidationParameters = GetTokenSignatureValidationParameters();

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }


        private SecurityKey GetSymmetricSecurityKey()
        {
            string secretKey = _config["JwtAuthentication:SecretKey"]; 
            byte[] symmetricKey = Convert.FromBase64String(secretKey);

            return new SymmetricSecurityKey(symmetricKey);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {

            return new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSymmetricSecurityKey(),

                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = _config["JwtAuthentication:Issuer"],

                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        }

        private TokenValidationParameters GetTokenSignatureValidationParameters()
        {

            return new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSymmetricSecurityKey(),

                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = _config["JwtAuthentication:Issuer"],

                ClockSkew = TimeSpan.FromMinutes(5)
            };
        }

        public JToken DeserializeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = (JwtSecurityToken)handler.ReadToken(token);
            var claims = jwtSecurityToken.Claims;

            var jsonToken = new JObject();
            foreach (var claim in claims)
                jsonToken.Add(claim.Type, claim.Value);

            return jsonToken;
        }

        public JwtSecurityToken HandleToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadToken(token) as JwtSecurityToken;
        }

        public async Task<AccountDto> GetApplicationUserDto(string token)
        {
            string userId = GetUserId(token);

            return _mapper.Map<AccountDto>(await _accountRepository.Get(Guid.Parse(userId)));
        }

        public string GetUserId(string token)
        {
            return GetTokenClaims(token).Where(s => s.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
        }
    }
}
