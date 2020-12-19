using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Common;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Common.Models.DTOs.Common;
using AutoMapper;
using System.Linq;

namespace AuthenticationServer.Infrastructure.Mappings
{
    class ContractToDtoProfile : Profile
    {
        public ContractToDtoProfile()
        {
            CreateMap<AbstractAccount, AbstractAccountDto>();
            CreateMap<AbstractAccountDto, AbstractAccount>();

            CreateMap<Tenant, TenantDto>()
                .ForMember(destination => destination.Languages,
                               options => options.MapFrom(source => source.Languages.Select(languageName => new LanguageDto() { Name = languageName }).ToList()))
                .ForMember(destination => destination.DashboardModel,
                               options => options.MapFrom(source => new DashboardDto() { Model = source.DashboardSchema }))
                .ForMember(destination => destination.UserSchema,
                               options => options.MapFrom(source => new UserSchemaDto() { DataModel = source.DataModelSchema, TrackModel = source.TrackModelSchema }));

            CreateMap<TenantDto, Tenant>();

            CreateMap<JwtConfigurationDto, JwtConfiguration>();
            CreateMap<JwtConfiguration, JwtConfigurationDto>();

            CreateMap<DomainName, DomainDto>()
                .ForMember(destination => destination.LogoDto,
                            options => options.MapFrom(source => new LogoDto() { Base64 = source.Logo }));
            CreateMap<DomainDto, DomainName>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
        }




    }
}
