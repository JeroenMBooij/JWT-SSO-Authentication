using System;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class LanguageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string RfcCode3066 { get; set; }

    }
}
