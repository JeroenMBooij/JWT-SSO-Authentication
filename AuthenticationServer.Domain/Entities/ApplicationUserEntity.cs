using AuthenticationServer.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    public class ApplicationUserEntity : IdentityUser<Guid>
    {
        public ApplicationUserEntity()
        {
            Tenants = new HashSet<ApplicationUserEntity>();
            Assets = new HashSet<ApplicationEntity>();
            Languages = new HashSet<LanguageEntity>();
        }
        
        [Required]
        [Column]
        public string Firstname { get; set; }

        [Column]
        public string Middlename { get; set; }

        [Column]
        [Required]
        public string Lastname { get; set; }

        [Column]
        [Required]
        public string AuthenticationRole { get; set; }

        [Column]
        public Guid? AdminId { get; set; }

        [Column]
        public DateTime Created { get; set; } = DateTime.Now;

        [Column]
        public DateTime? LastModified { get; set; }


        public virtual ApplicationUserEntity Admin { get; set; }
        public virtual ICollection<ApplicationUserEntity> Tenants { get; set; }

        [Required]
        public virtual ICollection<LanguageEntity> Languages { get; set; }

        public virtual ICollection<ApplicationEntity> Assets { get; set; }

        public virtual ICollection<RoleEntity> Roles { get; set; }




    }
}
