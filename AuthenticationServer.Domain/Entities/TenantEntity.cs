using AuthenticationServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    [Table("Tenants")]
    public class TenantEntity
    {
        public TenantEntity()
        {
            Domains = new HashSet<DomainEntity>();
            Languages = new HashSet<LanguageEntity>();
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool EmailVerified { get; set; }

        [Required]
        public string Passwordhash { get; set; }

        [Required]
        public string Firstname { get; set; }

        public string Middlename { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public virtual JwtConfigurationEntity UsersJwtConfiguration { get; set; }

        [Required]
        public virtual ICollection<DomainEntity> Domains { get; set; }

        [Required]
        public virtual ICollection<LanguageEntity> Languages { get; set; }

        [Required]
        public virtual UserSchemaEntity UserSchema { get; set; }
        [Required]
        public virtual DashboardEntity DashboardModel { get; set; }
    }
}
