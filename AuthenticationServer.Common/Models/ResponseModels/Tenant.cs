using AuthenticationServer.Common.Models.ResponseModels.Common;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthenticationServer.Common.Models.ResponseModels
{
    public class Tenant : Account
    {
        public List<User> Users { get; set; }
        public List<Domain> Domains { get; set; }
        public override Claim[] Claims
        {
            get
            {
                List<Claim> claims = DefaultClaims;

                return DefaultClaims.ToArray();
            }
        }
    }
}
