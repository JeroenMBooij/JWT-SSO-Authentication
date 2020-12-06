using AuthenticationServer.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    [Table("tenants")]
    public class TenantEntity : AuditableData
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public List<DomainEntity> Domains { get; set; }
    }
}
