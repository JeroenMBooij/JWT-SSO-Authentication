using AuthenticationServer.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    [Table("JwtConfigurations")]
    public class JwtConfigurationEntity : AuditableData
    {

        [Key]
        [Column]
        public Guid TenantId { get; set; }

        [Column]
        [Required]
        public long ExpireHours { get; set; }

        [Required]
        public virtual ApplicationUserEntity Tenant { get; set; }

    }
}
