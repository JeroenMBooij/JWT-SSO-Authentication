using AuthenticationServer.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class JwtConfig
    {
        public string SecretKey { get; set; }
        public double? ExpireMinutes { get; set; }
        public long? RefreshExpireMinutes { get; set; }
        public bool ValidateIssuer { get; set; }
        public string Issuer { get; set; }
        public SecurityAlgorithm Algorithm { get; set; }
    }
}
