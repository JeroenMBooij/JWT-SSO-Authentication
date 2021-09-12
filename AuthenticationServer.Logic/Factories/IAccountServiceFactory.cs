using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Logic.Factories
{
    public interface IAccountServiceFactory
    {
        IAccountService CreateAccountService(AccountRole accountService);

        Task<IAccountService> CreateAccountService(string email);
        Task<IAccountService> CreateAccountService(Guid Id);

        Task<AccountRole> IdentifyAccount(string email);
        Task<AccountRole> IdentifyAccount(Guid Id);
    }
}