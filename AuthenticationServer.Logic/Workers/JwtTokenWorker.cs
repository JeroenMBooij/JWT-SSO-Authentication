using AuthenticationServer.Common.Constants.Token;
using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationServer.Logic.Workers
{
    public class JwtTokenWorker : IJwtTokenWorker
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public JwtTokenWorker(IConfiguration config, ILogger<JwtTokenWorker> logger)
        {
            _config = config;
            _logger = logger;
        }

        public JwtTokenWorker(string startup, IConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public string GenerateToken(JwtModelDto model)
        {
            if (model == null || model.Claims == null || model.Claims.Length == 0)
                throw new ArgumentException("Arguments to create tokens are not valid");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _config["JWT_ISSUER"],
                Subject = new ClaimsIdentity(model.Claims),
                SigningCredentials = new SigningCredentials(GetSecurityKey(model.SecurityAlgorithm, model.SecretKey), model.SecurityAlgorithm.ToSchema())
            };

            if (model.ExpireMinutes.HasValue)
                securityTokenDescriptor.Expires = DateTime.Now.AddMinutes(model.ExpireMinutes.Value);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.SetDefaultTimesOnTokenCreation = false;
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            string token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return token;
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwt = handler.ReadJwtToken(token);

            return jwt.Claims;
        }

        public IEnumerable<Claim> GetTokenClaimsWithValidation(JwtTenantConfigDto jwtTentantConfig, string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters(jwtTentantConfig);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return tokenValid.Claims;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenInvalidTypeException();
            }
        }

        public bool IsTokenValid(JwtTenantConfigDto jwtTentantConfig, string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty.");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters(jwtTentantConfig);

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

        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty.");

            var jwtconfig = new JwtConfig()
            {
                SecretKey = _config["JWT_SECRETKEY"],
                ExpireMinutes = double.Parse(_config["JwtAdminConfig:ExpireMinutes"]),
                Algorithm = Enum.Parse<SecurityAlgorithm>(_config["JwtAdminConfig:Algorithm"]),
                ValidateIssuer = true,
                Issuer = _config["JWT_ISSUER"]
            };

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters(jwtconfig);

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

        public bool IsTokenSignatureValid(JwtTenantConfigDto jwtTenantConfigDto, string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty.");

            TokenValidationParameters tokenValidationParameters = GetTokenSignatureValidationParameters(jwtTenantConfigDto);

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

        public SecurityKey GetSecurityKey(SecurityAlgorithm algorithm, string secretKey)
        {
            SecurityDescription securityDescription = SupportedAlgorithms.List.Where(s => s.Name == algorithm).FirstOrDefault();

            switch (securityDescription.Type)
            {
                case EncryptionType.Asymmetric:
                    return GetAsymmetricSecurityKey(secretKey);

                default:
                    return GetSymmertricSecurityKey(secretKey);
            }
        }

        public SecurityKey GetAsymmetricSecurityKey(string secretKey)
        {
            _logger.LogInformation($"secretKey: {secretKey}");
            _logger.LogInformation($"RSA: {RSA.Create()}");
            RSA rsa = RSA.Create();
            rsa.ImportFromPem(secretKey.ToCharArray());

            var keyparameters = rsa.ExportParameters(true);

            var key = new RsaSecurityKey(keyparameters);

            return key;
        }

        public SecurityKey GetSymmertricSecurityKey(string secretKey)
        {
            // byte[] symmetricKey = Convert.FromBase64String(secretKey);
            byte[] symmetricKey = Encoding.UTF8.GetBytes(secretKey);

            return new SymmetricSecurityKey(symmetricKey);
        }

        private byte[] GetBytesFromPEM(string pemString, string section)
        {
            var header = $"-----BEGIN {section}-----";
            var footer = $"-----END {section}-----";

            var start = pemString.IndexOf(header, StringComparison.Ordinal);
            if (start < 0)
                return null;

            start += header.Length;
            var end = pemString.IndexOf(footer, start, StringComparison.Ordinal) - start;

            if (end < 0)
                return null;

            byte[] result = Convert.FromBase64String(pemString.Substring(start, end));
            return result;
        }

        public TokenValidationParameters GetTokenValidationParameters(JwtConfig jwtConfig)
        {

            return new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(jwtConfig.Algorithm, jwtConfig.SecretKey),

                ValidateAudience = false,
                ValidateIssuer = false,

                RequireExpirationTime = jwtConfig.ExpireMinutes.HasValue,
                ValidateLifetime = jwtConfig.ExpireMinutes.HasValue,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        }

        private TokenValidationParameters GetTokenSignatureValidationParameters(JwtTenantConfigDto jwtTenantConfigDto)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(jwtTenantConfigDto.Algorithm, jwtTenantConfigDto.SecretKey),

                ValidateAudience = false,
                ValidateIssuer = jwtTenantConfigDto.ValidateIssuer,

                ClockSkew = TimeSpan.FromMinutes(5)
            };

            if (jwtTenantConfigDto.ValidateIssuer)
                tokenValidationParameters.ValidIssuer = jwtTenantConfigDto.Issuer;

            return tokenValidationParameters;
        }

        public JToken DeserializeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            IEnumerable<Claim> claims = jwtSecurityToken.Claims;

            var jsonToken = new JObject();
            foreach (var claim in claims)
                jsonToken.Add(claim.Type, claim.Value);

            return jsonToken;
        }

        public Guid GetUserId(string token)
        {
            string userId = GetTokenClaims(token).Where(s => s.Type == "uid").FirstOrDefault().Value;

            return Guid.Parse(userId);
        }


    }
}
