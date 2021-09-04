using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

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

            CreateMap<JwtConfigurationEntity, JwtConfigurationDto>();
            CreateMap<JwtConfigurationDto, JwtConfigurationEntity>();

            CreateMap<LanguageEntity, LanguageDto>();
            CreateMap<LanguageDto, LanguageEntity>();

            CreateMap<DashboardEntity, DashboardDto>();
            CreateMap<DashboardDto, DashboardEntity>();
        }

    }
}
