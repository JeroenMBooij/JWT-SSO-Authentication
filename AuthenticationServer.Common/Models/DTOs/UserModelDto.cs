using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class UserModelDto
    {
        public Guid UserId { get; set; }
        public Guid SchemaTenantId { get; set; }
        public JToken UserDataModel { get; set; }



        public UserDto User { get; set; }
        public UserSchemaDto Schema { get; set; }
    }
}
