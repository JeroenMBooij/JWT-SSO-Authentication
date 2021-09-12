using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.ContractModels.Applications;
using AuthenticationServer.Common.Models.ContractModels.Token;
using AuthenticationServer.Common.Models.DTOs;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace AuthenticationServer.Infrastructure.Mappings
{
    class ContractToDtoProfile : Profile
    {
        public ContractToDtoProfile()
        {
            CreateMap<AccountRegistration, AccountDto>()
                .ForMember(destination => destination.Languages,
                               options => options.MapFrom(source => source.Languages.Select(languageName => new LanguageDto() { Name = languageName }).ToList()))
                .ForMember(destination => destination.ConfigData,
                               options => options.MapFrom(source => JObject.Parse(source.ConfigData)));
            CreateMap<AccountDto, AccountRegistration>()
                .ForMember(destination => destination.Languages,
                               options => options.MapFrom(source => source.Languages.Select(s => s.Name).ToList()))
                .ForMember(destination => destination.ConfigData,
                               options => options.MapFrom(source => JsonConvert.SerializeObject(source.ConfigData)));


            CreateMap<AdminAccount, AccountDto>()
                .ForMember(destination => destination.Languages,
                               options => options.MapFrom(source => source.Languages.Select(languageName => new LanguageDto() { Name = languageName }).ToList()))
                .ForMember(destination => destination.ConfigData,
                               options => options.MapFrom(source => JObject.Parse(source.ConfigData)));
            CreateMap<AccountDto, AdminAccount>()
                .ForMember(destination => destination.Languages,
                               options => options.MapFrom(source => source.Languages.Select(s => s.Name).ToList()))
                .ForMember(destination => destination.ConfigData,
                               options => options.MapFrom(source => JsonConvert.SerializeObject(source.ConfigData)));


            CreateMap<Application, ApplicationDto>()
                .ForMember(destination => destination.LogoDto,
                            options => options.MapFrom(source => new LogoDto() { Base64 = source.Logo }));

            CreateMap<ApplicationDto, Application>();
            CreateMap<ApplicationDto, ApplicationWithId>()
                .ReverseMap();

            CreateMap<JwtTenantConfig, JwtTenantConfigDto>()
                .ReverseMap();

            CreateMap<DomainName, DomainNameDto>()
                .ReverseMap();


            CreateMap<Role, RoleDto>()
                .ReverseMap();

        }




    }
}
