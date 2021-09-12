using AuthenticationServer.Common.Models.ContractModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace AuthenticationServer.Common.Models.DTOs
{
    // TODO Look into seperating this into Tenant and Admin with automapper
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string AdminToken { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public JObject ConfigData { get; set; }
        public bool LockoutEnabled { get; set; }
        public string AuthenticationRole { get; set; }
        public List<string> Roles { get; set; }
        public Guid? AdminId { get; set; }
        public List<LanguageDto> Languages { get; set; }
        public AccountDto Admin { get; set; }
        public List<AccountDto> Tenants { get; set; }

        public List<ApplicationDto> Assets { get; set; }

        
    }
}
