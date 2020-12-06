using System;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.Domain.Common
{
    public class AuditableData
    {
        [Required]
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
