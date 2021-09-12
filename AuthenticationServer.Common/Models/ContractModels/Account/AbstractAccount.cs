using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Models.DTOs;
using AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthenticationServer.Common.Models.ContractModels.Account
{
    public abstract class AbstractAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfigData { get; set; }
        public List<string> Languages { get; set; }

    }
}
