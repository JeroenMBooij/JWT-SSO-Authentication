using AuthenticationServer.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationServer.Domain.Entities
{
    [Table("jwt_configuration")]
    public class JwtConfigurationEntity : AuditableData
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTimeOffset Nbf { get; set; }
        [Required]
        public DateTimeOffset Exp { get; set; }

    }
}
