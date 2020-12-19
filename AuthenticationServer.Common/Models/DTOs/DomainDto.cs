using System;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class DomainDto
    {
        private TenantDto _tenant;

        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Url { get; set; }
        public LogoDto LogoDto { get; set; }
        public TenantDto Tenant
        {
            get => _tenant;
            set
            {
                TenantId = value.Id;
                _tenant = value;
            }
        }


    }
}
