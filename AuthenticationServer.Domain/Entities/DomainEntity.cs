using AuthenticationServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    [Table("Domains")]
    public class DomainEntity : AuditableData
    {
        public DomainEntity()
        {
            User = new HashSet<UserEntity>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public Guid TenantId { get; set; }
        public string LogoLocation { get; set; }



        [Required]
        public virtual ICollection<UserEntity> User { get; set; }

        [Required]
        public virtual TenantEntity Tenant { get; set; }


    }
}
