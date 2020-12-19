using AuthenticationServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    [Table("Users")]
    public class UserEntity : AuditableData
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Firstname { get; set; }

        public string Middlename { get; set; }

        public string Lastname { get; set; }

        [Required]
        public virtual UserModelEntity UserModel { get; set; }

        [Required]
        public virtual ICollection<RoleEntity> Roles { get; set; }

        [Required]
        public virtual ICollection<DomainEntity> Domains { get; set; }

        [Required]
        public virtual ICollection<LanguageEntity> Languages { get; set; }



    }
}
