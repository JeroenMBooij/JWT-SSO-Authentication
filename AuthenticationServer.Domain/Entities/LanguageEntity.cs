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
            Tenants = new HashSet<ApplicationUserEntity>();
        }

        [Key]
        [Column]
        public Guid Id { get; set; }

        [Column]
        [Required]
        public string Name { get; set; }

        [Column]
        [Required]
        public string Code { get; set; }

        [Column]
        [Required]
        public string RfcCode3066 { get; set; }

        [Required]
        public virtual ICollection<ApplicationUserEntity> Tenants { get; set; }

    }
}
