using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.ContractModels.Account;
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
        public JToken DataModel { get; set; }

    }
}
