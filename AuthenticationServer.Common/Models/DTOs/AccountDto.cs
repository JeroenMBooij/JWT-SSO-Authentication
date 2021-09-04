using AuthenticationServer.Common.Models.ContractModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string AdminToken { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string AuthenticationRole { get; set; }
        public List<string> Roles { get; set; }
        public Guid? AdminId { get; set; }
        public List<LanguageDto> Languages { get; set; }
        public AccountDto Admin { get; set; }
        public List<AccountDto> Tenants { get; set; }

        public List<ApplicationDto> Assets { get; set; }

        public Claim[] Claims 
        {
            get
            {
                return AccountClaims.ToArray();
            }  
        }


        private List<Claim> AccountClaims
        {
            get
            {
                return new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, Email),
                    new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName, Firstname),
                    new Claim(JwtRegisteredClaimNames.FamilyName, Lastname),
                    new Claim(ClaimTypes.Role, AuthenticationRole),
                    new Claim("GivenRoles", JsonConvert.SerializeObject(Roles)),
                    new Claim("SupportedLanguages", JsonConvert.SerializeObject(Languages.Select(languageDto => new Language()
                    {
                        Name = languageDto.Name,
                        Code = languageDto.Code,
                        RfcCode3066 = languageDto.RfcCode3066
                    })))
                };
            }
        }
    }
}
