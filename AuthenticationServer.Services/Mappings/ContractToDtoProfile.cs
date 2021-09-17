using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.ContractModels.Applications;
using AuthenticationServer.Common.Models.ContractModels.Token;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Common.Models.DTOs.Account;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace AuthenticationServer.Services.Mappings
{
    class ContractToDtoProfile : Profile
    {
        public ContractToDtoProfile()
        {
            CreateMap<AccountData, TenantAccountDto>()
                .ForMember(destination => destination.ConfigData,
                               options => options.MapFrom(source => JObject.Parse(source.ConfigData)));
            CreateMap<TenantAccountDto, AccountData>()
                .ForMember(destination => destination.ConfigData,
                               options => options.MapFrom(source => JsonConvert.SerializeObject(source.ConfigData)));

            CreateMap<AccountRegistration, AdminAccountDto>()
                .ForMember(destination => destination.ConfigData,
                               options => options.MapFrom(source => JObject.Parse(source.ConfigData)))
                .ForMember(destination => destination.AuthenticationRole,
                                options => options.MapFrom(source => Enum.Parse<AccountRole>(source.AuthenticationRole)));
            CreateMap<AdminAccountDto, AccountRegistration>()
                .ForMember(destination => destination.ConfigData,
                               options => options.MapFrom(source => JsonConvert.SerializeObject(source.ConfigData)))
                .ForMember(destination => destination.AuthenticationRole,
                                options => options.MapFrom(source => source.AuthenticationRole.ToString()));

            CreateMap<AccountRegistration, TenantAccountDto>()
                .ForMember(destination => destination.ConfigData,
                               options => options.MapFrom(source => JObject.Parse(source.ConfigData)))
                .ForMember(destination => destination.AuthenticationRole,
                                options => options.MapFrom(source => Enum.Parse<AccountRole>(source.AuthenticationRole)))
                .ForMember(destination => destination.AdminId,
                                options => options.MapFrom(source => Guid.Parse(source.AdminId)));
            CreateMap<TenantAccountDto, AccountRegistration>()
                .ForMember(destination => destination.ConfigData,
                               options => options.MapFrom(source => JsonConvert.SerializeObject(source.ConfigData)))
                .ForMember(destination => destination.AuthenticationRole,
                                options => options.MapFrom(source => source.AuthenticationRole.ToString()))
                .ForMember(destination => destination.AdminId,
                                options => options.MapFrom(source => source.AdminId.ToString()));



            CreateMap<AccountWithId, TenantAccountDto>()
                .ForMember(destination => destination.ConfigData,
                               options => options.MapFrom(source => JObject.Parse(source.ConfigData)))
                .ForMember(destination => destination.Id,
                                options => options.MapFrom(source => Guid.Parse(source.Id)));
            CreateMap<TenantAccountDto, AccountWithId>()
                .ForMember(destination => destination.ConfigData,
                               options => options.MapFrom(source => JsonConvert.SerializeObject(source.ConfigData)))
                .ForMember(destination => destination.Id,
                                options => options.MapFrom(source => source.Id.ToString()));


            CreateMap<Application, ApplicationDto>()
                .ForMember(destination => destination.IconUUID,
                                options => options.MapFrom(source => Guid.Parse(source.IconUUID)))
                .ForAllMembers(opt => opt.Condition((source, dest, sourceMember, destMember) => (sourceMember != null)));
            CreateMap<ApplicationDto, Application>()
                .ForMember(destination => destination.IconUUID,
                                options => options.MapFrom(source => source.IconUUID.ToString()));

            CreateMap<ApplicationWithId, ApplicationDto>()
                .ForMember(destination => destination.Id,
                                options => options.MapFrom(source => Guid.Parse(source.Id)))
                .ForMember(destination => destination.IconUUID,
                                options => options.MapFrom(source => Guid.Parse(source.IconUUID)));

            CreateMap<JwtTenantConfig, JwtTenantConfigDto>()
                .ForMember(destination => destination.Algorithm,
                            options => options.MapFrom(source => Enum.Parse<SecurityAlgorithm>(source.Algorithm)))
                .ForAllMembers(opt => opt.Condition((source, dest, sourceMember, destMember) => (sourceMember != null)));
            CreateMap<JwtTenantConfigDto, JwtTenantConfig>()
                .ForMember(destination => destination.Algorithm,
                            options => options.MapFrom(source => source.Algorithm.ToString()));

            CreateMap<DomainName, DomainNameDto>()
                .ForAllMembers(opt => opt.Condition((source, dest, sourceMember, destMember) => (sourceMember != null)));
            CreateMap<DomainNameDto, DomainName>();


            CreateMap<Role, RoleDto>()
                .ReverseMap();

        }




    }
}
