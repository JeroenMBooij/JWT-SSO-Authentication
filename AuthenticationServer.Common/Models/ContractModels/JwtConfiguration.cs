using System;

namespace AuthenticationServer.Common.Models.ContractModels
{
    public class JwtConfiguration
    {
        public int ExpireMinutes { get; set; }
        public DateTimeOffset Nbf { get; set; }
        public DateTimeOffset Exp { get; set; }
    }
}