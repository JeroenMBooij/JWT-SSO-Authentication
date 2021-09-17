using AuthenticationServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    [Table("Applications")]
    public class ApplicationEntity : AuditableData
    {
        public ApplicationEntity()
        {
            JwtTenantConfigurations = new HashSet<JwtTenantConfigEntity>();
            Domains = new HashSet<DomainNameEntity>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column]
        public string Name { get; set; }

        [Required]
        [Column]
        public Guid AdminId { get; set; }

        [Column]
        public string MultimediaUUID { get; set; }

        [Required]
        public virtual ApplicationUserEntity Admin { get; set; }

        [Required]
        public virtual ICollection<DomainNameEntity> Domains { get; set; }

        [Required]
        public virtual ICollection<JwtTenantConfigEntity> JwtTenantConfigurations { get; set; }

    }
}
