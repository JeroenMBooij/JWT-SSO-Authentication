using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AutoMapper;

namespace AuthenticationServer.Infrastructure.Mappings
{
    public class DtoToEntityProfile : Profile
    {
        public DtoToEntityProfile()
        {
            CreateMap<TenantEntity, TenantDto>();
            CreateMap<TenantDto, TenantEntity>();

            CreateMap<DomainEntity, DomainDto>();
            CreateMap<DomainDto, DomainEntity>();

            CreateMap<UserEntity, UserDto>();
            CreateMap<UserDto, UserEntity>();

            CreateMap<JwtConfigurationEntity, JwtConfigurationDto>();
            CreateMap<JwtConfigurationDto, JwtConfigurationEntity>();

            CreateMap<RoleEntity, RoleDto>();
            CreateMap<RoleDto, RoleEntity>();

            CreateMap<LanguageEntity, LanguageDto>();
            CreateMap<LanguageDto, LanguageEntity>();

            CreateMap<UserModelEntity, UserModelDto>();
            CreateMap<UserModelDto, UserModelEntity>();

            CreateMap<UserSchemaEntity, UserSchemaDto>();
            CreateMap<UserSchemaDto, UserSchemaEntity>();

            CreateMap<DashboardEntity, DashboardDto>();
            CreateMap<DashboardDto, DashboardEntity>();
        }

    }
}
