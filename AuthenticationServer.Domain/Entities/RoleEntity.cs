using AuthenticationServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    [Table("Roles")]
    public class RoleEntity : AuditableData
    {
        public RoleEntity()
        {
            Users = new HashSet<UserEntity>();
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual ICollection<UserEntity> Users { get; set; }
       
    }
}
