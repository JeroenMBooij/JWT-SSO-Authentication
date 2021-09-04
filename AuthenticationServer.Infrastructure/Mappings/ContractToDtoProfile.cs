using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.DTOs;
using AutoMapper;
using System.Collections.Generic;
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
                .ReverseMap();

            CreateMap<ApplicationAccount, AccountDto>()
                .ForMember(destination => destination.Languages,
                               options => options.MapFrom(source => source.Languages.Select(languageName => new LanguageDto() { Name = languageName }).ToList()))
                .ForMember(destination => destination.Assets,
                               options => options.MapFrom(source => new List<ApplicationDto>() { new ApplicationDto() { Name = source.Asset.Name, Url = source.Asset.Url } }))
                .ReverseMap();


            CreateMap<JwtConfigurationDto, JwtConfiguration>();
            CreateMap<JwtConfiguration, JwtConfigurationDto>();

            CreateMap<DomainName, ApplicationDto>()
                .ForMember(destination => destination.LogoDto,
                            options => options.MapFrom(source => new LogoDto() { Base64 = source.Logo }));
            CreateMap<ApplicationDto, DomainName>();


            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
        }




    }
}
