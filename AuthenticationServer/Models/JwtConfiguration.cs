using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models
{
    public class JwtConfiguration
    {
        public int Id { get; set; }
        public string APIKey { get; set; }
        public DateTimeOffset Nbf { get; set; }
        public DateTimeOffset Exp { get; set; }

    }
}
