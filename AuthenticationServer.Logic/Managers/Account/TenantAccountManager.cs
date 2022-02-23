using AuthenticationServer.Common.Constants.Token;
using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.ContractModels.Token;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Common.Models.DTOs.Account;
using AuthenticationServer.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Workers.Account
{
    public class TenantAccountManager : AbstractAccountManager, ITenantAccountManager
    {
        private readonly ITenantAccountRepository _tenantRepo;
        private readonly IApplicationRepository _applicationRepo;
        private readonly IJwtTenantConfigRepository _jwtTenantConfigRepo;
        private readonly IConfiguration _config;

        public TenantAccountManager(IMapper mapper, IJwtTokenWorker jwtManager, ITenantAccountRepository tenantRepo,
            IApplicationRepository applicationRepo, IJwtTenantConfigRepository jwtTenantConfigRepo, IEmailService emailManager,
            IConfiguration config)
            : base(mapper, jwtManager, emailManager, tenantRepo, config)
        {
            _tenantRepo = tenantRepo;
            _applicationRepo = applicationRepo;
            _jwtTenantConfigRepo = jwtTenantConfigRepo;
            _config = config;
        }


        public bool IsTokenValid(string token)
        {
            return _jwtManager.IsTokenValid(null, token);
        }

        public async Task<Ticket> LoginAsync(Credentials credentials, Guid applicationId)
        {
            Guid? adminId = await _accountRepository.GetAdminIdByEmail(credentials.Email);

            var applicationDto = _mapper.Map<ApplicationDto>(await _applicationRepo.Get(adminId.Value, applicationId));

            if (applicationDto == null)
                throw new AuthenticationApiException("Login", $"{applicationId} is an invalid applicationId");

            return await LoginAsync(credentials, applicationDto);
        }

        public async Task<Ticket> LoginAsync(Credentials credentials, string hostname)
        {
            ApplicationDto applicationDto;
            if (string.IsNullOrEmpty(credentials.ApplicationId) == false)
            {
                applicationDto = _mapper.Map<ApplicationDto>(await _applicationRepo.Get(null, Guid.Parse(credentials.ApplicationId)));
                if (applicationDto == null)
                    throw new AuthenticationApiException("Login", $"Invalid applicationId provided");
            }
            else
            {
                applicationDto = _mapper.Map<ApplicationDto>(await _applicationRepo.GetApplicationFromHostname(hostname));
                if (applicationDto == null)
                    throw new AuthenticationApiException("Login", $"No application registered as {hostname}");
            }


            return await LoginAsync(credentials, applicationDto);
        }

        private async Task<Ticket> LoginAsync(Credentials credentials, ApplicationDto applicationDto)
        {
            AbstractAccountDto tenantDto = await LoginAsync<TenantAccountDto>(credentials.Email, credentials.Password);

            JwtModelDto jwtModelDto = await CreateJwtModelAsync(applicationDto.Id, tenantDto);


            Ticket ticket = await CreateAccessTicket(tenantDto.Id, applicationDto.Id, jwtModelDto);

            return ticket;
        }

        public async Task<string> RegisterAsync(AccountRegistration tenant)
        {
            if (tenant.AdminId is null)
                throw new AuthenticationApiException("AccountData", "AdminId is required for Tenant Accounts");

            ApplicationDto applicationDto = _mapper.Map<ApplicationDto>(await _applicationRepo.Get(Guid.Parse(tenant.AdminId), Guid.Parse(tenant.ApplicationId)));

            if(applicationDto is null)
                throw new AuthenticationApiException("AccountData", "Invalid ApplicationId provided");


            TenantAccountDto tenantDto = _mapper.Map<TenantAccountDto>(tenant);
            tenantDto.AdminId = applicationDto.AdminId;
            //TODO multiple jwt configurations on 1 application identifier
            tenantDto.LockoutEnabled = applicationDto.JwtTenantConfigurations.FirstOrDefault().LockoutEnabled;

            return await RegisterTenantAsync(tenantDto);
        }

        public async Task<string> RegisterWithHostnameAsync(AccountData tenant, string hostname)
        {
            ApplicationDto applicationDto = _mapper.Map<ApplicationDto>(await _applicationRepo.GetApplicationFromHostname(hostname));

            TenantAccountDto tenantAccount = _mapper.Map<TenantAccountDto>(tenant);
            tenantAccount.AdminId = applicationDto.AdminId;
            //TODO multiple jwt configurations on 1 application identifier
            tenantAccount.LockoutEnabled = applicationDto.JwtTenantConfigurations.FirstOrDefault().LockoutEnabled;

            return await RegisterTenantAsync(tenantAccount);
        }

        public async Task<string> RegisterWithTokenAsync(AccountData tenant, string adminToken)
        {
            Guid adminId = _jwtManager.GetUserId(adminToken);

            TenantAccountDto tenantAccount = _mapper.Map<TenantAccountDto>(tenant);
            tenantAccount.AdminId = adminId;
            tenantAccount.LockoutEnabled = true;

            return await RegisterTenantAsync(tenantAccount);
        }

        private async Task<string> RegisterTenantAsync(TenantAccountDto AccountDto)
        {
            AccountDto = await CreateAccountAsync(AccountDto);

            if (_config["NETWORK_ENVIRONMENT"] != "Standalone")
            {
                await _emailManager.SendVerificationEmail(AccountDto.Email, AccountDto.Id);
                return $"An Email has been send to {AccountDto.Email}. Please confirm your Email to complete your registration.";
            }
            else
            {
                await _tenantRepo.SetVerified(AccountDto.Id);
                return $"Email verified you can now login. Deploy email service to send emails https://github.com/JeroenMBooij/EmailService and remove the NETWORK_ENVIRONMENT variable from docker-compose";
            }


        }

        protected async Task<TenantAccountDto> CreateAccountAsync(TenantAccountDto tenantDto)
        {
            PopulateAccountPropertiesForNewAccountAsync(tenantDto);

            var account = _mapper.Map<ApplicationUserEntity>(tenantDto);

            await _accountRepository.Insert(account, tenantDto.Password);

            return tenantDto;
        }


        public override async Task<JwtModelDto> CreateJwtModelAsync<T>(Guid? applicationId, T accountDto)
        {
            if (applicationId is null || accountDto is null)
                throw new ArgumentException();

            JwtTenantConfigDto jwtTenantConfigDto = _mapper.Map<JwtTenantConfigDto>(await _jwtTenantConfigRepo.GetFromApplicationId(applicationId.Value));

            var model = new JwtModelDto();
            model.SecretKey = jwtTenantConfigDto.SecretKey;
            model.SecurityAlgorithm = jwtTenantConfigDto.Algorithm;
            model.ExpireMinutes = jwtTenantConfigDto.ExpireMinutes;
            model.RefreshExpireMinutes = jwtTenantConfigDto.RefreshExpireMinutes;

            List<Claim> claims = new List<Claim>();
            foreach (ClaimConfig claimConfig in jwtTenantConfigDto.Claims)
            {
                claims.Add(new Claim(claimConfig.JwtName, getConfiguredValue(claimConfig, accountDto, jwtTenantConfigDto.ExpireMinutes)));
            }

            claims.Add(new Claim("uid", accountDto.Id.ToString()));

            model.Claims = claims.ToArray();

            return model;
        }

        protected override void PopulateAccountPropertiesForNewAccountAsync(AbstractAccountDto tenantDto)
        {
            tenantDto.Id = Guid.NewGuid();
            tenantDto.AuthenticationRole = AccountRole.Tenant;
        }




        private string getConfiguredValue(ClaimConfig claimConfig, AbstractAccountDto accountDto, double? expireMinutes)
        {
            switch (claimConfig.Type)
            {
                case ClaimType.ConfigData:
                    return accountDto.ConfigData[claimConfig.Data].ToString();

                case ClaimType.HardData:
                    return claimConfig.Data;

                case ClaimType.Email:
                    return accountDto.Email;

                case ClaimType.JWTExp:
                    return new DateTimeOffset(DateTime.Now)
                        .AddMinutes(expireMinutes.Value).ToUnixTimeSeconds().ToString();

                case ClaimType.JWTIat:
                    return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString();

                case ClaimType.Array:
                    JArray claims = new JArray();
                    foreach (ClaimConfig subClaimConfig in claimConfig.ClaimConfigurations)
                    {
                        var subClaim = new JObject();
                        subClaim[subClaimConfig.JwtName] = getConfiguredValue(subClaimConfig, accountDto, expireMinutes);
                        claims.Add(subClaim);
                    }

                    return JsonConvert.SerializeObject(claims);

                default:
                    throw new NotImplementedException();
            }
        }

    }
}
