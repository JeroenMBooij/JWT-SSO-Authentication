using System;
using System.Collections.Generic;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class ApplicationDto
    {
        private AbstractAccountDto _tenant;

        public Guid Id { get; set; }
        public Guid AdminId { get; set; }
        public string Name { get; set; }
        public List<DomainNameDto> Domains { get; set; }
        public Guid? IconUUID { get; set; }
        public AbstractAccountDto Tenant
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

        public List<JwtTenantConfigDto> JwtTenantConfigurations { get; set; }


    }
}
