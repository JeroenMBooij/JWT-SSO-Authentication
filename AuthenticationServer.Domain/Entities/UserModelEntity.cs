using AuthenticationServer.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    [Table("UserModels")]
    public class UserModelEntity : AuditableData
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public Guid SchemaTenantId { get; set; }

        [Required]
        public string UserDataModel { get; set; }



        public virtual UserEntity User { get; set; }
        public virtual UserSchemaEntity Schema { get; set; }

    }
}
