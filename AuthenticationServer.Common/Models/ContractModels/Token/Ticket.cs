using System;

namespace AuthenticationServer.Common.Models.ContractModels.Token
{
    public class Ticket
    {
        public string RegisteredJWT { get; set; }
        public string RegisteredRefreshToken { get; set; }
        public string JwtIssuedAt { get; set; }
        public string ApplicationId { get; set; }
    }
}
