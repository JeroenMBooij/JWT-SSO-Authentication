using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationServer.Service.Token
{
    public class TokenProcessService : ITokenProcessService
    {
        private readonly IJwtManager _jwtManager;
        private readonly IConfiguration _config;

        public TokenProcessService(IJwtManager jwtManager, IConfiguration config)
        {
            _jwtManager = jwtManager;
            _config = config;
        }

        public JToken Deserialize(string token)
        {
            ValidateToken(token);

            return _jwtManager.DeserializeToken(token);
        }

        public bool ValidateToken(string token)
        {
            return _jwtManager.IsTokenValid(token);
        }
    }
}
