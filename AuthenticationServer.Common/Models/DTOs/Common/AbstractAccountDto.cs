using System;
using System.Collections.Generic;

namespace AuthenticationServer.Common.Models.DTOs.Common
{
    public abstract class AbstractAccountDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Passwordhash { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public List<LanguageDto> Languages { get; set; }


    }
}
