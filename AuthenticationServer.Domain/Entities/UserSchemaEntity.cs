using AuthenticationServer.Domain.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    [Table("UserSchemas")]
    public class UserSchemaEntity : AuditableData
    {
        public UserSchemaEntity()
        {
            UserModels = new HashSet<UserModelEntity>();
        }

        [Key]
        public Guid TenantId { get; set; }

        [Required]
        public string DataModel { get; set; }

        [Required]
        public string TrackModel { get; set; }

        public virtual TenantEntity Tenant { get; set; }
        public virtual ICollection<UserModelEntity> UserModels { get; set; }



    }
}
