using AuthenticationServer.Common.Models.ResponseModels.Common;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;

namespace AuthenticationServer.Common.Models.ResponseModels
{
    public class User : Account
    {
        public Tenant Tenant { get; set; }
        public List<Role> Roles { get; set; }
        public override Claim[] Claims
        {
            get
            {
                var claims = DefaultClaims;

                Roles.ForEach(role => claims.Add(new Claim(type: "Roles", value: JsonSerializer.Serialize(Roles))));

                return claims.ToArray();
            }
        }
    }
}
