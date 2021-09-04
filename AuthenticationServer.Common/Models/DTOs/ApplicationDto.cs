using System;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class ApplicationDto
    {
        private AccountDto _tenant;

        public Guid Id { get; set; }
        public Guid AdminId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public LogoDto LogoDto { get; set; }
        public AccountDto Tenant
        {
            get => _tenant;
            set
            {
                if (value != null)
                {
                    AdminId = value.Id;
                    _tenant = value;
                }
            }
        }


    }
}
