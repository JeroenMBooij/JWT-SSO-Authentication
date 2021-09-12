using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;

namespace AuthenticationServer.Infrastructure.Mappings
{
    public class DtoToEntityProfile : Profile
    {
        public DtoToEntityProfile()
        {
           CreateMap<AccountDto, ApplicationUserEntity>()
                .ForMember(destination => destination.NormalizedEmail,
                               options => options.MapFrom(source => source.Email.ToUpper()))
                .ForMember(destination => destination.UserName,
                               options => options.MapFrom(source => source.Email))
                .ForMember(destination => destination.Roles,
                                options => options.MapFrom(source => source.Roles.Select(roleName => new RoleEntity(roleName)) ) )
                .ForMember(destination => destination.ConfigData,
                            options => options.MapFrom(source => JsonConvert.SerializeObject(source.ConfigData)));

            CreateMap<ApplicationUserEntity, AccountDto>()
                .ForMember(destination => destination.Roles,
                                options => options.MapFrom(source => source.Roles.Select(s => s.Name)))
                .ForMember(destination => destination.ConfigData,
                            options => options.MapFrom(source => JObject.Parse(source.ConfigData)));

            CreateMap<ApplicationEntity, ApplicationDto>()
                .ReverseMap();

            CreateMap<LanguageEntity, LanguageDto>()
                .ReverseMap();

            CreateMap<DomainNameEntity, DomainNameDto>()
               .ReverseMap();

            CreateMap<JwtTenantConfigEntity, JwtTenantConfigDto>()
                .ForMember(destination => destination.Algorithm,
                    options => options.MapFrom(source => Enum.Parse(typeof(SecurityAlgorithm), source.Algorithm)))
                .ForMember(destination => destination.Claims,
                    options => options.MapFrom(source => JsonConvert.DeserializeObject<List<ClaimConfig>>(source.Claims)));
            CreateMap<JwtTenantConfigDto, JwtTenantConfigEntity > ()
                .ForMember(destination => destination.Algorithm,
                    options => options.MapFrom(source => source.Algorithm.ToString()))
                .ForMember(destination => destination.Claims,
                    options => options.MapFrom(source => JsonConvert.SerializeObject(source.Claims)));
        }

        private List<Claim> StringArrayToClaims(string stringClaims)
        {
            List<dynamic> list = JsonConvert.DeserializeObject<List<dynamic>>(stringClaims);

            return list.Select(s => new Claim(type: s.Type.ToString(), value: s.Value.ToString())).ToList();
        }

        private string ClaimsToStringArray(List<Claim> claims)
        {
            List<dynamic> list = claims.Select(s => (object)new { Type = s.Type, Value = s.Value }).ToList();

            return JsonConvert.SerializeObject(list);
        }
    }
}
