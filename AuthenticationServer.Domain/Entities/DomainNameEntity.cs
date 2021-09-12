using AuthenticationServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Domain.Entities
{
    [Table("DomainName")]
    public class DomainNameEntity : AuditableData
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column]
        public string Name { get; set; }

        [Required]
        [Column]
        public string Url { get; set; }

        [Required]
        [Column]
        public Guid ApplicationId { get; set; }


        [Required]
        public virtual ApplicationEntity Application { get; set; }
    }
}
