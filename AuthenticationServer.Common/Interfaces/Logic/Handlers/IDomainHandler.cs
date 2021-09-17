using AuthenticationServer.Common.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Logic.Handlers
{
    public interface IDomainHandler
    {
        Task SetLanguageDtoProperties(LanguageDto languageDto);
        Task<ApplicationDto> GetDomainFromUrl(string url);
        Task<ApplicationDto> GetDomainFromName(string name);
        Task<List<ApplicationDto>> GetDomainsFromAdminToken(string adminToken);
        Task AddTenantToAdmin(AbstractAccountDto tenantDto);
    }
}
