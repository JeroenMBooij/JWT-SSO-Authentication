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
        private readonly IJwtManager _jwtManager;
        private readonly ITenantAccountManager _tenantAccountManager;
        private readonly IMapper _mapper;

        public TenantAccountService(IJwtManager jwtManager, ITenantAccountManager accountManager, IMapper mapper)
        {
            _jwtManager = jwtManager;
            _tenantAccountManager = accountManager;
            _mapper = mapper;
        }



        public async Task<string> RegisterTenantAsync(Tenant tenant)
        {
            await _tenantAccountManager.CreateTenantAccountAsync(_mapper.Map<TenantDto>(tenant));

            //TODO email confirmation
            return $"An Email has been send to {tenant.Email}. Please confirm your Email to complete your registration.";
        }

    }
}
