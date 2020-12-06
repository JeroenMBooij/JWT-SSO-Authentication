using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthenticationServer.Common.Models.ResponseModels.Common
{
    public abstract class Account
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Password { get; set; }
        public abstract Claim[] Claims { get; }
        protected List<Claim> DefaultClaims
        {
            get
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, Name));
                claims.Add(new Claim(ClaimTypes.Email, Email));
                claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()));

                return claims;
            }

        }
    }
}
