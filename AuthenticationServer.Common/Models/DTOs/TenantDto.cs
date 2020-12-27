using AuthenticationServer.Common.Models.DTOs.Common;
using System.Collections.Generic;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class TenantDto : AbstractAccountDto
    {
        public JwtConfigurationDto UsersJwtConfiguration { get; set; }
        public List<DomainDto> Domains { get; set; }
        public UserSchemaDto UserSchema { get; set; }
        public DashboardDto DashboardModel { get; set; }

    }
}
