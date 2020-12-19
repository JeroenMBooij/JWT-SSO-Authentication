using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Common
{
    public class AuditableData
    {
        [Column("created")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        [Column("last_modified")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastModified { get; set; }
    }
}
