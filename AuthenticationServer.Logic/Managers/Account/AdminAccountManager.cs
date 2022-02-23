using AuthenticationServer.Common.Enums;
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
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Workers.Account
{
    public class AdminAccountManager : AbstractAccountManager, IAdminAccountManager
    {
        private readonly IConfiguration _config;

        public AdminAccountManager(IMapper mapper, IJwtTokenWorker jwtManager, IAdminAccountRepository applicationAccountRepo,
            IConfiguration config, IEmailService emailManager)
            : base(mapper, jwtManager, emailManager, applicationAccountRepo, config)
        {
            _config = config;
        }

        public async Task<Ticket> LoginAsync(Credentials credentials, string hostname = "")
        {
            AdminAccountDto adminDto = await LoginAsync<AdminAccountDto>(credentials.Email, credentials.Password);

            JwtModelDto jwtModelDto = await CreateJwtModelAsync(null, adminDto);

            Ticket ticket = await CreateAccessTicket(adminDto.Id, null, jwtModelDto);

            return ticket; ;
        }

        public async Task<Ticket> LoginAsync(Credentials credentials, Guid applicationId)
        {
            return await LoginAsync(credentials);
        }

        public async Task<string> RegisterAsync(AccountRegistration adminAccount)
        {
            AdminAccountDto adminAccountDto = _mapper.Map<AdminAccountDto>(adminAccount);


            await CreateAccountAsync(adminAccountDto);

            if (_config["NETWORK_ENVIRONMENT"] != "Standalone")
            {
                await _emailManager.SendVerificationEmail(adminAccountDto.Email, adminAccountDto.Id);
                return $"An Email has been send to {adminAccountDto.Email}. Please confirm your Email to complete your registration.";
            }
            else
            {
                await _accountRepository.SetVerified(adminAccountDto.Id);
                return $"Email verified you can now login. Deploy email service to send emails https://github.com/JeroenMBooij/EmailService and remove the NETWORK_ENVIRONMENT variable from docker-compose";
            }
        }


        protected async Task<AdminAccountDto> CreateAccountAsync(AdminAccountDto tenantDto)
        {
            PopulateAccountPropertiesForNewAccountAsync(tenantDto);

            var account = _mapper.Map<ApplicationUserEntity>(tenantDto);

            await _accountRepository.Insert(account, tenantDto.Password);

            return tenantDto;
        }


        public override Task<JwtModelDto> CreateJwtModelAsync<T>(Guid? applicationId, T accountDto)
        {
            JwtModelDto model = _config.GetSection("JwtAdminConfig")
                                                                .Get<JwtModelDto>();

            model.Claims = new Claim[5];
            model.Claims[0] = new Claim("uid", accountDto.Id.ToString());
            model.Claims[1] = new Claim(ClaimTypes.Role, AccountRole.Admin.ToString());
            model.Claims[2] = new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString());
            model.Claims[3] = new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now).AddMinutes(model.ExpireMinutes.Value).ToUnixTimeSeconds().ToString());
            model.Claims[4] = new Claim(JwtRegisteredClaimNames.Iss, _config["JWT_ISSUER"]);

            model.SecretKey = _config["JWT_SECRETKEY"];
            return Task.FromResult(model);
        }

        protected override void PopulateAccountPropertiesForNewAccountAsync(AbstractAccountDto adminDto)
        {
            adminDto.Id = Guid.NewGuid();
            adminDto.LockoutEnabled = true;
        }
    }
}
