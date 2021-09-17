using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.DTOs.Account;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Workers
{
    public class TenantStatisticsManager : ITenantStatisticsManager
    {
        private readonly ITenantAccountRepository _tenantAccountRepo;
        private readonly IJwtTokenWorker _jwtManager;
        private readonly IMapper _mapper;

        public TenantStatisticsManager(ITenantAccountRepository tenantAccountRepo, IJwtTokenWorker jwtManager, IMapper mapper)
        {
            _tenantAccountRepo = tenantAccountRepo;
            _jwtManager = jwtManager;
            _mapper = mapper;
        }

        public async Task<PaginatedAccounts> GetTenantsPaginated(string token, int startIndex, int endIndex)
        {
            Guid adminId = _jwtManager.GetUserId(token);

            List<TenantAccountDto> tenantAccountDtos = _mapper.Map<List<TenantAccountDto>>(await _tenantAccountRepo.GetByAdmin(adminId));

            var paginatedAccounts = new PaginatedAccounts()
            {
                TotalItems = tenantAccountDtos.Count,
                Range = new PageRange()
                {
                    From = startIndex + 1,
                    To = endIndex + 1
                },
                TotalPages = (long)Math.Ceiling(decimal.ToDouble(tenantAccountDtos.Count / (endIndex - startIndex))) + 1,
                CurrentPage = ((startIndex - 1) / endIndex - startIndex) + 1,
                Items = _mapper.Map<List<AccountWithId>>(tenantAccountDtos.Skip(startIndex).Take((endIndex - startIndex) + 1))
            };


            return paginatedAccounts;
        }

    }
}
