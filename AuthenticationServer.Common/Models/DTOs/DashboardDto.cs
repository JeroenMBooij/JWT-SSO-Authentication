using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.Common.Models.DTOs
{
    public class DashboardDto
    {
        private AccountDto _tenant;

        public Guid TenantId { get; set; }

        [Required]
        public JToken Model { get; set; }

        public AccountDto Tenant
        {
            get => _tenant;
            set
            {
                TenantId = value.Id;
                _tenant = value;
            }
        }
    }
}