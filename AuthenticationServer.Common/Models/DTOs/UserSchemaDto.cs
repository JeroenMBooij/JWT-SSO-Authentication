using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class UserSchemaDto
    {
        private TenantDto _tenant;


        public Guid TenantId { get; set; }
        public JToken DataModel { get; set; }
        public JToken TrackModel { get; set; }
        public TenantDto Tenant 
        { 
            get => _tenant;
            set
            {
                TenantId = value.Id;
                _tenant = value;
            }
        }
        public List<UserModelDto> UserModels { get; set; } = new List<UserModelDto>();
    }
}
