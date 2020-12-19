using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.DTOs.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class UserDto : AbstractAccountDto
    {
        public UserModelDto UserModel { get; set; }
        public JToken DataModel { get; set; }
        public List<RoleDto> Roles { get; set; }
        public List<DomainDto> Domains { get; set; }
        public string CurrentLanguageCode { get; set; }
        public string CurrentUrl { get; set; }
        public Claim[] Claims { get; set; }

        
    }
}
