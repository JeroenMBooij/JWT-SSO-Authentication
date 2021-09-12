using AuthenticationServer.Common.Constants.Token;
using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.DTOs;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Services.Account
{
    public class TenantAccountService : AbstractAccountService, ITenantAccountService
    {
        private readonly IJwtManager _jwtManager;
        private readonly ITenantAccountRepository _tenantRepo;
        private readonly IApplicationRepository _applicationRepo;
        private readonly IJwtTenantConfigRepository _jwtTenantConfigRepo;

        public TenantAccountService(IMapper mapper, IJwtManager jwtManager, ITenantAccountRepository tenantRepo,
            ILanguageRepository languageRepo, IApplicationRepository applicationRepo, IJwtTenantConfigRepository jwtTenantConfigRepo,
            IEmailManager emailManager)
            : base(mapper, emailManager, tenantRepo, languageRepo)
        {
            _jwtManager = jwtManager;
            _tenantRepo = tenantRepo;
            _applicationRepo = applicationRepo;
            _jwtTenantConfigRepo = jwtTenantConfigRepo;
        }


        public bool IsTokenValid(string token)
        {
            return _jwtManager.IsTokenValid(null, token);
        }

        public async Task<string> LoginAsync(Credentials credentials, Guid applicationId)
        {
            ApplicationDto applicationDto = _mapper.Map<ApplicationDto>
                (await _applicationRepo.Get(applicationId));

            return await LoginAsync(credentials, applicationDto);
        }

        public async Task<string> LoginAsync(Credentials credentials, string hostname)
        {
            ApplicationDto applicationDto = _mapper.Map<ApplicationDto>
                (await _applicationRepo.GetApplicationFromHostname(hostname));

            return await LoginAsync(credentials, applicationDto);
        }

        private async Task<string> LoginAsync(Credentials credentials, ApplicationDto applicationDto)
        {
            AccountDto tenantDto = await LoginAsync(credentials.Email, credentials.Password);

            JwtModelDto jwtConfigurationDto = await CreateJwtConfigurationAsync(applicationDto.Id, tenantDto);

            string token = _jwtManager.GenerateToken(jwtConfigurationDto);

            return token;
        }


        public async Task<string> RegisterWithHostnameAsync(AccountRegistration tenant, string hostname)
        {
            ApplicationDto applicationDto = _mapper.Map<ApplicationDto>(await _applicationRepo.GetApplicationFromHostname(hostname));

            AccountDto tenantAccount = _mapper.Map<AccountDto>(tenant);
            tenantAccount.AdminId = applicationDto.AdminId;
            //TODO multiple jwt configurations on 1 application identifier
            tenantAccount.LockoutEnabled = applicationDto.JwtTenantConfigurations.FirstOrDefault().LockoutEnabled;

            return await RegisterTenantAsync(tenantAccount);
        }

        public async Task<string> RegisterWithTokenAsync(AccountRegistration tenant, string adminToken)
        {
            Guid adminId = _jwtManager.GetUserId(adminToken);

            AccountDto tenantAccount = _mapper.Map<AccountDto>(tenant);
            tenantAccount.AdminId = adminId;
            tenantAccount.LockoutEnabled = true;

            return await RegisterTenantAsync(tenantAccount);
        }

        private async Task<string> RegisterTenantAsync(AccountDto AccountDto)
        {
            AccountDto = await CreateAccountAsync(AccountDto);

            if (Debugger.IsAttached == false)
                await _emailManager.SendVerificationEmail(AccountDto.Email, AccountDto.Id);
            else
                await _tenantRepo.SetVerified(AccountDto.Id);


            return $"An Email has been send to {AccountDto.Email}. Please confirm your Email to complete your registration.";
        }

        protected override async Task<JwtModelDto> CreateJwtConfigurationAsync(Guid? applicationId, AccountDto accountDto)
        {
            if (applicationId is null || accountDto is null)
                throw new ArgumentException();

            JwtTenantConfigDto jwtTenantConfigDto = _mapper.Map<JwtTenantConfigDto>(await _jwtTenantConfigRepo.GetFromApplicationId(applicationId.Value));

            var model = new JwtModelDto();
            model.SecretKey = jwtTenantConfigDto.SecretKey;
            model.SecurityAlgorithm = jwtTenantConfigDto.Algorithm;
            model.ExpireMinutes = jwtTenantConfigDto.ExpireMinutes;

            List<Claim> claims = new List<Claim>();
            foreach (ClaimConfig claimConfig in jwtTenantConfigDto.Claims)
            {
                claims.Add(new Claim(claimConfig.JwtName, getConfiguredValue(claimConfig, accountDto, jwtTenantConfigDto.ExpireMinutes)));
            }

            claims.Add(new Claim("uid", accountDto.Id.ToString()));

            model.Claims = claims.ToArray();

            return model;
        }

        protected override async Task PopulateAccountPropertiesForNewAccountAsync(AccountDto tenantDto)
        {
            tenantDto.Id = Guid.NewGuid();
            tenantDto.AuthenticationRole = AccountRole.Tenant.ToString();

            await PopulateLanguagePropertiesForNewAccountAsync(tenantDto);
        }




        private string getConfiguredValue(ClaimConfig claimConfig, AccountDto accountDto, double? expireMinutes)
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
                    List<dynamic> claims = new List<dynamic>();
                    foreach (ClaimConfig subClaimConfig in claimConfig.ClaimConfigurations)
                    {
                        var subClaim = new ExpandoObject();
                        ((IDictionary<string, object>)subClaim)[subClaimConfig.JwtName] = getConfiguredValue(subClaimConfig, accountDto, expireMinutes);
                        claims.Add(subClaim);
                    }

                    return JsonConvert.SerializeObject(claims);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
