using AuthenticationServer.Domain.Common;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AuthenticationServer.Domain.Entities
{
    [Table("Dashboards")]
    public class DashboardEntity : AuditableData
    {
        [Key]
        [Column]
        public Guid TenantId { get; set; }

        [Column]
        [Required]
        public string Model { get; set; }

        public virtual ApplicationUserEntity Tenant { get; set; }

        //[NotMapped]
        //[Required]
        //public JToken Model
        //{
        //    get
        //    {
        //        return JsonConvert.DeserializeObject<JObject>(_Model);
        //    }
        //    set
        //    {
        //        _Model = JsonConvert.SerializeObject(value);
        //    }
        //}
    }
}
