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
        [Column("id")]
        public Guid TenantId { get; set; }

        [Required]
        [Column("nbf")]
        public DateTimeOffset Nbf { get; set; }

        [Required]
        [Column("exp")]
        public DateTimeOffset Exp { get; set; }

        [Required]
        public virtual TenantEntity Tenant { get; set; }

    }
}
