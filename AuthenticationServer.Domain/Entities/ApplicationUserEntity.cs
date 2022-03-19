using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    [Table("ApplicationUsers")]
    public class ApplicationUserEntity : IdentityUser<Guid>
    {
        public ApplicationUserEntity()
        {
            Tenants = new HashSet<ApplicationUserEntity>();
            Assets = new HashSet<ApplicationEntity>();
            Roles = new HashSet<RoleEntity>();
        }

        [Required]
        [Column]
        public string ConfigData { get; set; }

        [Column]
        [Required]
        public string AuthenticationRole { get; set; }

        [Column]
        public string RegisteredJWT { get; set; }

        [Column]
        public DateTime? JwtIssuedAt { get; set; }

        [Column]
        public string RegisteredRefreshToken { get; set; }

        [Column]
        public string RegisteredApplication { get; set; }

        [Column]
        public Guid? AdminId { get; set; }

        [Column]
        public DateTime Created { get; set; } = DateTime.Now;

        [Column]
        public DateTime? LastModified { get; set; }


        public virtual ApplicationUserEntity Admin { get; set; }
        public virtual ICollection<ApplicationUserEntity> Tenants { get; set; }

        public virtual ICollection<ApplicationEntity> Assets { get; set; }

        public virtual ICollection<RoleEntity> Roles { get; set; }




    }
}
