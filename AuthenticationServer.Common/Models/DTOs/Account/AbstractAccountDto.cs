using AuthenticationServer.Common.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AuthenticationServer.Common.Models.DTOs
{
    public abstract class AbstractAccountDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public JObject ConfigData { get; set; }
        public AccountRole AuthenticationRole { get; set; }
        public List<ApplicationDto> Assets { get; set; }
        public bool LockoutEnabled { get; set; }


    }
}
