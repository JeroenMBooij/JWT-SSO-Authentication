using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.ContractModels.Common;
using AutoMapper;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AuthenticationServer.Common.Models.ContractModels
{
    public class User : AbstractAccount
    {
        public List<Role> Roles { get; set; }
        public JToken DataModel { get; set; }

    }
}
