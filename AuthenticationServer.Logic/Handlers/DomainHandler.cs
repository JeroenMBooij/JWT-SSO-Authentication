using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Handlers;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Models.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Handlers
{
    public class DomainHandler : IDomainHandler
    {
        private readonly IMapper _mapper;
        private readonly ILanguageRepository _languageRepository;
        private readonly IApplicationRepository _domainRepository;
        private readonly IJwtManager _jwtManager;

        public DomainHandler(IMapper mapper, ILanguageRepository languageRepository, IApplicationRepository domainRepository, IJwtManager jwtManager)
        {
            _mapper = mapper;
            _languageRepository = languageRepository;
            _domainRepository = domainRepository;
            _jwtManager = jwtManager;
        }

        public async Task SetLanguageDtoProperties(LanguageDto languageDto)
        {
            LanguageDto databaseLanguageDto = _mapper.Map<LanguageDto>(await _languageRepository.GetLanguageByName(languageDto.Name));
            languageDto.Id = databaseLanguageDto.Id;
            languageDto.Code = databaseLanguageDto.Code;
            languageDto.RfcCode3066 = databaseLanguageDto.RfcCode3066;
        }

        public async Task<ApplicationDto> GetDomainFromUrl(string url)
        {
            return _mapper.Map<ApplicationDto>(await _domainRepository.GetDomainFromUrl(url));
        }

        public async Task<ApplicationDto> GetDomainFromName(string name)
        {
            return _mapper.Map<ApplicationDto>(await _domainRepository.GetDomainFromName(name));
        }

        public async Task<List<ApplicationDto>> GetDomainsFromAdminToken(string adminToken)
        {
            string adminId = _jwtManager.GetUserId(adminToken);

            return _mapper.Map<List<ApplicationDto>>(await _domainRepository.GetDomainsFromAdminId(adminId));
        }

        public async Task AddTenantToAdmin(AccountDto tenantDto)
        {
            AccountDto admin = await _jwtManager.GetApplicationUserDto(tenantDto.AdminToken);

            tenantDto.AdminId = admin.Id;
            tenantDto.Admin = admin;
        }
    }
}
