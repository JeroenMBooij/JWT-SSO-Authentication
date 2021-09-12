using AuthenticationServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Domain.Entities
{
    [Table("JwtTenantConfig")]
    public class JwtTenantConfigEntity : AuditableData
    {
        [Key]
        public Guid Id { get; set; }

        [Column]
        [Required]
        public string SecretKey { get; set; }

        [Column]
        [Required]
        public string Claims { get; set; }

        [Column]
        public long? ExpireMinutes { get; set; }

        [Column]
        [Required]
        public string Algorithm { get; set; }

        [Required]
        public virtual ApplicationEntity Application { get; set; }
    }
}
