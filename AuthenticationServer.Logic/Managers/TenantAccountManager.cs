using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Handlers;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Managers
{
    public class TenantAccountManager : ITenantAccountManager
    {
        private readonly IPasswordHandler _passwordHandler;
        private readonly ITenantRepository _tenantRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IMapper _mapper;

        public TenantAccountManager(IPasswordHandler passwordHandler, ITenantRepository tenantRepository, 
            ILanguageRepository languageRepository, IMapper mapper)
        {
            _passwordHandler = passwordHandler;
            _tenantRepository = tenantRepository;
            _languageRepository = languageRepository;
            _mapper = mapper;
        }

        public async Task CreateTenantAccountAsync(TenantDto tenantDto)
        {
            //TODO catch duplicate GUID
            await PopulateTenantPropertiesForNewAccountAsync(tenantDto);

            await _tenantRepository.Insert(_mapper.Map<TenantEntity>(tenantDto));

            //TODO Confirm Email
        }

        private async Task PopulateTenantPropertiesForNewAccountAsync(TenantDto tenantDto)
        {
            tenantDto.Id = Guid.NewGuid();
            tenantDto.Passwordhash = _passwordHandler.HashPassword(tenantDto.Password);
            tenantDto.UserSchema.Tenant = tenantDto;
            tenantDto.DashboardModel.Tenant = tenantDto;

            foreach(DomainDto domainDto in tenantDto.Domains)
            {
                domainDto.Id = Guid.NewGuid();
                domainDto.Tenant = tenantDto;
            }

            foreach (LanguageDto languageDto in tenantDto.Languages)
            {
                LanguageDto databaseLanguageDto = _mapper.Map<LanguageDto>(await _languageRepository.GetLanguageByName(languageDto.Name));
                languageDto.Id = databaseLanguageDto.Id;
                languageDto.Code = databaseLanguageDto.Code;
                languageDto.RfcCode3066 = databaseLanguageDto.RfcCode3066;
            }

                
        }
    }
}
