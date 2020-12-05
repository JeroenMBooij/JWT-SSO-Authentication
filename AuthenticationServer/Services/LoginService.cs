using AuthenticationServer.Logic.Managers;
using AuthenticationServer.Logic.Models;
using AuthenticationServer.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationServer.Services
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _config;

        public LoginService(IConfiguration config)
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
            JWTContainerModel jwtContainerModel = _config.GetSection("JWTSecurity").Get<JWTContainerModel>();

            var jwtManager = new JWTManager(jwtContainerModel.SecretKey);

            return jwtManager.IsTokenValid(token);
        }
    }
}
