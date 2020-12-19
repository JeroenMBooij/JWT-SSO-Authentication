using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.ContractModels.Common;
using AutoMapper;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.Common.Models.ContractModels
{
    public class Tenant : AbstractAccount
    {
        public JwtConfiguration UsersJwtConfiguration { get; set; }
        public List<DomainName> Domains { get; set; }
        public JToken DataModelSchema { get; set; }
        public JToken TrackModelSchema { get; set; }
        public JToken DashboardSchema { get; set; }

    }
}
