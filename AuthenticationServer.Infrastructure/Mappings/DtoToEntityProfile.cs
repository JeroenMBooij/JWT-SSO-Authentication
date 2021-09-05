using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AutoMapper;
using Newtonsoft.Json;
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
                                options => options.MapFrom(source => source.Roles.Select(roleName => new RoleEntity(roleName)) ) );

            CreateMap<ApplicationUserEntity, AccountDto>()
                .ForMember(destination => destination.Roles,
                                options => options.MapFrom(source => source.Roles.Select(s => s.Name)));

            CreateMap<ApplicationEntity, ApplicationDto>();
            CreateMap<ApplicationDto, ApplicationEntity>();

            CreateMap<JwtConfigurationEntity, JwtConfigurationDto>()
                .ForMember(destination => destination.ConfiguredClaims,
                    options => options.MapFrom(source => StringArrayToClaims(source.ConfiguredClaims)));
            CreateMap<JwtConfigurationDto, JwtConfigurationEntity>()
                .ForMember(destination => destination.ConfiguredClaims,
                    options => options.MapFrom(source => ClaimsToStringArray(source.ConfiguredClaims)));

            CreateMap<LanguageEntity, LanguageDto>();
            CreateMap<LanguageDto, LanguageEntity>();

            CreateMap<DashboardEntity, DashboardDto>();
            CreateMap<DashboardDto, DashboardEntity>();
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
