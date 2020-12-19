using AuthenticationServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AuthenticationServer.Domain.Entities
{
    [Table("Languages")]
    public class LanguageEntity : AuditableData
    {
        public LanguageEntity()
        {
            Tenants = new HashSet<TenantEntity>();
            Users = new HashSet<UserEntity>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string RfcCode3066 { get; set; }

        [Required]
        public virtual ICollection<TenantEntity> Tenants { get; set; }

        [Required]
        public virtual ICollection<UserEntity> Users { get; set; }
    }
}
