using AuthenticationServer.Common.Constants.AccountManager;
using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Handlers;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Managers.Account
{
    public abstract class AccountManager
    {
        protected readonly IMapper _mapper;
        protected readonly ITenantRepository _tenantRepository;
        protected readonly IDomainHandler _domainHandler;
        protected readonly IJwtConfigurationRepository _jwtConfigurationRepository;

        //TODO implement CQRS with MediatR package
        public AccountManager(IMapper mapper, ITenantRepository tenantRepository,
            IDomainHandler domainHandler, IJwtConfigurationRepository jwtConfigurationRepository)
        {
            _tenantRepository = tenantRepository;
            _domainHandler = domainHandler;
            _jwtConfigurationRepository = jwtConfigurationRepository;
            _mapper = mapper;
        }

        public async Task<AccountDto> CreateAccountAsync(AccountDto tenantDto)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    await PopulateAccountPropertiesForNewAccountAsync(tenantDto);

                    var test = new RoleEntity(Roles.Admin);

                    var account = _mapper.Map<ApplicationUserEntity>(tenantDto);
                    await _tenantRepository.Insert(account, tenantDto.Password);
                    break;
                }
                catch (DbUpdateException updateException)
                {
                    attempts++;
                    if (attempts == 2)
                        throw updateException;
                }
            }

            return tenantDto;
        }

        public async Task<AccountDto> LoginAsync(string email, string password)
        {
            AccountDto tenantDto = _mapper.Map<AccountDto>(await _tenantRepository.GetTenantByEmail(email));
            if (tenantDto == null)
                throw new AuthenticationApiException("Tenant Login", "Invalid email address or password");

            SignInResult result = await _tenantRepository.SignInTenant(email, password);
            if (result.Succeeded)
                return tenantDto;

            if (result.IsLockedOut)
            {
                DateTimeOffset lockOutTimeOffset = (await _tenantRepository.GetTenantLockoutTime(email)).Value;
                TimeSpan timeLeft = lockOutTimeOffset.DateTime.Subtract(DateTime.UtcNow);
                string timeLeftMessage = timeLeft.TotalMinutes < 1 ? $"{timeLeft.Seconds} seconds" : $"{timeLeft.Minutes} minutes";

                throw new AuthenticationApiException("Tenant Login", $"There have been too many failed login attempts. Please wait for {timeLeftMessage} minutes to try again.");
            }

            if (result.IsNotAllowed)
                throw new AuthenticationApiException("Tenant Login", "Email has not been verified yet.");

            throw new AuthenticationApiException("Tenant Login", "Invalid email address or password");
        }

        public JwtConfigurationDto CreateTenantJwtConfiguration(AccountDto tenantDto)
        {
            JwtConfigurationDto jwtConfigurationDto = _mapper.Map<JwtConfigurationDto>(_jwtConfigurationRepository.GetTenantJwtContainerModel());
            jwtConfigurationDto.Claims = tenantDto.Claims;

            return jwtConfigurationDto;
        }

        public async Task ChangePassword(string email, string oldPassword, string newPassword)
        {
            IdentityResult result = await _tenantRepository.ChangePassword(email, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                foreach (var error in result.Errors)
                    errorResponse.Errors.Add(new ErrorModel() { FieldName = "Change Tenant Password", Message = error.Description });

                throw new AuthenticationApiException(errorResponse.Errors);
            }
        }

        public async Task<string> ResetPassword(string email)
        {
            return await _tenantRepository.ResetPassword(email);
        }

        public async Task RecoverPassword(string email, string resetToken, string newPassword)
        {
            IdentityResult result = await _tenantRepository.RecoverPassword(email, resetToken, newPassword);

            if (!result.Succeeded)
            {
                ErrorResponse errorResponse = new ErrorResponse();
                foreach (var error in result.Errors)
                    errorResponse.Errors.Add(new ErrorModel() { FieldName = "Recover Tenant Password", Message = error.Description });

                throw new AuthenticationApiException(errorResponse.Errors);
            }

        }

        protected async Task PopulateLanguagePropertiesForNewAccountAsync(AccountDto accountDto)
        {

            foreach (LanguageDto languageDto in accountDto.Languages)
            {
                // languageDto can already be set if we needed a new tenant Guid
                if (languageDto.Id == Guid.Empty)
                    await _domainHandler.SetLanguageDtoProperties(languageDto);
            }
        }

        protected abstract Task PopulateAccountPropertiesForNewAccountAsync(AccountDto tenantDto);
    }
}
