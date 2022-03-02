using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels.Token;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Common.Models.DTOs.Account;
using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Workers.Account
{
    public class TokenProcessManager : ITokenProcessManager
    {
        private readonly IMapper _mapper;
        private readonly IJwtTokenWorker _jwtManager;
        private readonly IRepository _repository;
        private readonly ITenantAccountManager _tenantAccountService;

        public TokenProcessManager(IMapper mapper, IJwtTokenWorker jwtManager, IRepository repository, ITenantAccountManager tenantAccountService)
        {
            _mapper = mapper;
            _jwtManager = jwtManager;
            _repository = repository;
            _tenantAccountService = tenantAccountService;
        }

        public async Task<JToken> Deserialize(string token)
        {
            await ValidateToken(token);

            return _jwtManager.DeserializeToken(token);
        }

        // TODO refactor
        public async Task<Ticket> RefreshToken(Ticket ticket)
        {
            Guid accountId = _jwtManager.GetUserId(ticket.RegisteredJWT);

            Ticket registeredTicket = await _repository.TenantAccount.GetTicket(accountId);
            if (registeredTicket is null)
                throw new AuthenticationApiException("refresh", "invalid");

            JwtTenantConfigDto jwtTenantConfigDto = _mapper.Map<JwtTenantConfigDto>(await _repository.JwtTenantConfig.GetFromApplicationId(Guid.Parse(ticket.ApplicationId)));

            DateTime refreshExpireDate = DateTime.Parse(registeredTicket.JwtIssuedAt).AddMinutes(jwtTenantConfigDto.RefreshExpireMinutes.Value);
            if (refreshExpireDate > DateTime.UtcNow)
                throw new AuthenticationApiException("refresh", "expired", 410);

            if (registeredTicket.RegisteredRefreshToken.Equals(ticket.RegisteredRefreshToken) &&
               registeredTicket.RegisteredJWT.Equals(ticket.RegisteredJWT))
            {
                var tenantDto = _mapper.Map<TenantAccountDto>(await _repository.TenantAccount.Get(null, accountId));

                JwtModelDto jwtModelDto = await _tenantAccountService.CreateJwtModelAsync(Guid.Parse(ticket.ApplicationId), tenantDto);

                ticket.RegisteredJWT = _jwtManager.GenerateToken(jwtModelDto);

                await _repository.TenantAccount.RegisterTicket(tenantDto.Id, ticket);

                return ticket;
            }

            throw new AuthenticationApiException("Refresh Token", "Invalid");
        }

        public async Task<bool> ValidateToken(string token)
        {
            Guid accountId = _jwtManager.GetUserId(token);

            var account = await _repository.TenantAccount.Get(null, accountId);

            if (account.AuthenticationRole == "Admin")
                return _jwtManager.IsTokenValid(token);
            else
            {
                JwtTenantConfigDto jwtTenantConfigDto = _mapper.Map<JwtTenantConfigDto>(await _repository.JwtTenantConfig.GetFromApplicationId(Guid.Parse(account.RegisteredApplication)));
                return _jwtManager.IsTokenValid(jwtTenantConfigDto, token);
            }

        }
    }
}
