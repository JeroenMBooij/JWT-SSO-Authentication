using AuthenticationServer.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationServer.Service
{
    public class TokenProcessService : ITokenProcessService
    {
        private readonly IConfiguration _config;

        public TokenProcessService(IConfiguration config)
        {
            _config = config;
        }

        public JToken Deserialize(string token)
        {
            //TODO: improve validations exceptions
            if (!IsValid(token))
                throw new ArgumentException("Given token is not valid");

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = (JwtSecurityToken)handler.ReadToken(token);
            var claims = jwtSecurityToken.Claims;

            var jsonToken = new JObject();
            foreach (var claim in claims)
                jsonToken.Add(claim.Type, claim.Value);

            return jsonToken;
        }

        public bool IsValid(string token)
        {
            //JwtConfiguration jwtContainerModel = _config.GetSection("JWTSecurity").Get<JwtConfiguration>();

            //var jwtManager = new JWTManager(jwtContainerModel.SecretKey);

            return true; // jwtManager.IsTokenValid(token);
        }
    }
}
