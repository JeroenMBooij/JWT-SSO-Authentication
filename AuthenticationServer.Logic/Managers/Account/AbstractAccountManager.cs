using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Token;
using AuthenticationServer.Common.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Workers.Account
{
    public abstract class AbstractAccountManager
    {
        protected readonly IEmailService _emailManager;
        protected readonly IAccountRepository _accountRepository;
        protected readonly IMapper _mapper;
        protected readonly IJwtTokenWorker _jwtManager;

        public AbstractAccountManager(IMapper mapper, IJwtTokenWorker jwtManager, IEmailService emailManager, IAccountRepository accountRepository)
        {
            _emailManager = emailManager;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _jwtManager = jwtManager;
        }

        public async Task<T> LoginAsync<T>(string email, string password) where T : AbstractAccountDto
        {
            T tenantDto = _mapper.Map<T>(await _accountRepository.GetAccountByEmail(email));
            if (tenantDto == null)
                throw new AuthenticationApiException("login", "Invalid Credentials provided");

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

            throw new AuthenticationApiException("login", "Invalid Credentials provided");
        }

        protected async Task<Ticket> CreateAccessTicket(Guid accountId, Guid? applicationId, JwtModelDto jwtModelDto)
        {
            Ticket ticket = new Ticket();
            ticket.RegisteredJWT = _jwtManager.GenerateToken(jwtModelDto);

            if (jwtModelDto.RefreshExpireMinutes is not null)
            {
                ticket.RegisteredRefreshToken = Guid.NewGuid().ToString();

                await _accountRepository.RegisterTicket(accountId, ticket);
            }

            // TODO refactor
            if (applicationId is not null)
                await _accountRepository.RegisterJWT(accountId, applicationId, ticket.RegisteredJWT);

            return ticket;
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

        public async Task<string> VerifyEmail(Guid code)
        {
            AbstractAccountDto tenantDto;
            try
            {
                tenantDto = _mapper.Map<AbstractAccountDto>(await _accountRepository.Get(null, code));
            }
            catch (Exception)
            {
                throw new Exception("Bad Request contact your system administrator.");
            }

            await _accountRepository.SetVerified(tenantDto.Id);

            return tenantDto.Email;
        }

        public async Task<AbstractAccountDto> Get(Guid id)
        {
            return _mapper.Map<AbstractAccountDto>(await _accountRepository.Get(null, id));
        }

        public abstract Task<JwtModelDto> CreateJwtModelAsync<T>(Guid? applicationId, T accountDto) where T : AbstractAccountDto;
        protected abstract void PopulateAccountPropertiesForNewAccountAsync(AbstractAccountDto tenantDto);


    }
}
