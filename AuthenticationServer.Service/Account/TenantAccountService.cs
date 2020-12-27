using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Common.Models.ContractModels;
using System.Threading.Tasks;
using AutoMapper;
using AuthenticationServer.Common.Interfaces.Services;

namespace AuthenticationServer.Service.Account
{
    public class TenantAccountService : ITenantAccountService
    {
        private readonly IMapper _mapper;
        private readonly IJwtManager _jwtManager;
        private readonly ITenantAccountManager _tenantAccountManager;
        private readonly IEmailManager _emailManager;

        public TenantAccountService(IMapper mapper, IJwtManager jwtManager, 
            ITenantAccountManager accountManager, IEmailManager emailManager)
        {
            _jwtManager = jwtManager;
            _tenantAccountManager = accountManager;
            _emailManager = emailManager;
            _mapper = mapper;
        }

        public Task<string> LoginTenantAsync(Credentials credentials)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> RegisterTenantAsync(Tenant tenant)
        {
            TenantDto tenantDto = await _tenantAccountManager.CreateTenantAccountAsync(_mapper.Map<TenantDto>(tenant));

            await _emailManager.SendVerificationEmail(tenant.Email, tenantDto.Id);

            return $"An Email has been send to {tenant.Email}. Please confirm your Email to complete your registration.";
        }

    }
}
