using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Common
{
    public class AuditableData
    {
        [Required]
        [Column]
        public DateTime Created { get; set; } = DateTime.Now;

        [Column]
        public DateTime? LastModified { get; set; }


    }
}
