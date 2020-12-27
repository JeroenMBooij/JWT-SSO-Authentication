using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Common
{
    public class AuditableData
    {
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? LastModified { get; set; }


    }
}
