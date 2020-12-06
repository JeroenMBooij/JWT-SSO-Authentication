using AuthenticationServer.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    [Table("Domains")]
    public class DomainEntity : AuditableData
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string URL { get; set; }
        [Required]
        public List<UserEntity> User { get; set; }
        [Required]
        public TenantEntity Tenant { get; set; }

    }
}
