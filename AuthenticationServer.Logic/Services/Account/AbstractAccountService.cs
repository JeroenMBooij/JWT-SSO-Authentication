using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Services.Account
{
    public abstract class AbstractAccountService
    {
        protected readonly IEmailManager _emailManager;
        protected readonly IAccountRepository _accountRepository;
        private readonly ILanguageRepository _languageRepository;
        protected readonly IMapper _mapper;

        public AbstractAccountService(IMapper mapper, IEmailManager emailManager, IAccountRepository accountRepository,
            ILanguageRepository languageRepository)
        {
            _emailManager = emailManager;
            _accountRepository = accountRepository;
            _languageRepository = languageRepository;
            _mapper = mapper;
        }

        public async Task<AccountDto> LoginAsync(string email, string password)
        {
            AccountDto tenantDto = _mapper.Map<AccountDto>(await _accountRepository.GetAccountByEmail(email));
            if (tenantDto == null)
                throw new AuthenticationApiException("Tenant Login", "Invalid email address or password");

            SignInResult result = await _accountRepository.SignInAccount(email, password);
            if (result.Succeeded)
                return tenantDto;

            if (result.IsLockedOut)
            {
                DateTimeOffset lockOutTimeOffset = (await _accountRepository.GetLockoutTime(email)).Value;
                TimeSpan timeLeft = lockOutTimeOffset.DateTime.Subtract(DateTime.UtcNow);
                string timeLeftMessage = timeLeft.TotalMinutes < 1 ? $"{timeLeft.Seconds} seconds" : $"{timeLeft.Minutes} minutes";

                throw new AuthenticationApiException("Tenant Login", $"There have been too many failed login attempts. Please wait for {timeLeftMessage} minutes to try again.");
            }

            if (result.IsNotAllowed)
                throw new AuthenticationApiException("Tenant Login", "Email has not been verified yet.");

            throw new AuthenticationApiException("Tenant Login", "Invalid email address or password");
        }

        public async Task ChangePassword(NewCredentials nc)
        {
            IdentityResult result = await _accountRepository.ChangePassword(nc.Email, nc.OldPassword, nc.NewPassword);

            if (!result.Succeeded)
                throw new AuthenticationApiException(result.Errors.ToErrorModel());
        }

        public async Task ResetPassword(string email)
        {
            string passwordRecoverToken = await _accountRepository.ResetPassword(email);

            await _emailManager.SendRecoverPasswordEmail(email, passwordRecoverToken);
        }

        public async Task RecoverPassword(ResetPasswordModel rpm)
        {
            IdentityResult result = await _accountRepository.RecoverPassword(rpm.Email, rpm.ResetToken, rpm.NewPassword);

            if (!result.Succeeded)
                throw new AuthenticationApiException(result.Errors.ToErrorModel());

        }

        public async Task VerifyEmail(string code)
        {
            AccountDto tenantDto;
            try
            {
                tenantDto = _mapper.Map<AccountDto>(await _accountRepository.Get(Guid.Parse(code)));
            }
            catch (Exception)
            {
                throw new Exception("Bad Request contact your system administrator.");
            }

            await _accountRepository.SetVerified(tenantDto.Id);
        }

        public async Task<AccountDto> Get(Guid id)
        {
            return _mapper.Map<AccountDto>(await _accountRepository.Get(id));
        }

        protected async Task<AccountDto> CreateAccountAsync(AccountDto tenantDto)
        {
            int attempts = 0;
            while (true)
            {
                try
                {
                    await PopulateAccountPropertiesForNewAccountAsync(tenantDto);

                    var account = _mapper.Map<ApplicationUserEntity>(tenantDto);

                    await _accountRepository.Insert(account, tenantDto.Password);

                    break;
                }
                catch (DbUpdateException updateException)
                {
                    attempts++;
                    if (attempts == 2)
                        throw new Exception("guid error");
                }
            }

            return tenantDto;
        }

        protected async Task PopulateLanguagePropertiesForNewAccountAsync(AccountDto accountDto)
        {
            foreach (LanguageDto languageDto in accountDto.Languages)
            {
                LanguageDto databaseLanguageDto = _mapper.Map<LanguageDto>(await _languageRepository.GetLanguageByName(languageDto.Name));
                if (databaseLanguageDto is null)
                    throw new AuthenticationApiException("language", $"{languageDto.Name} is not a supported language");

                languageDto.Id = databaseLanguageDto.Id;
                languageDto.Code = databaseLanguageDto.Code;
                languageDto.RfcCode3066 = databaseLanguageDto.RfcCode3066;
            }
        }

        protected abstract Task<JwtModelDto> CreateJwtConfigurationAsync(Guid? applicationId, AccountDto accountDto);
        protected abstract Task PopulateAccountPropertiesForNewAccountAsync(AccountDto tenantDto);


    }
}
