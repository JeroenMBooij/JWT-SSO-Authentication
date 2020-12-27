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
        public Guid TenantId { get; set; }

        [Required]
        public DateTimeOffset Nbf { get; set; }

        [Required]
        public DateTimeOffset Exp { get; set; }

        [Required]
        public virtual TenantEntity Tenant { get; set; }

    }
}
