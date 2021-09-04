using AuthenticationServer.Logic.Managers.Account;

namespace AuthenticationServer.Logic.Factories
{
    public interface IAccountManagerFactory
    {
        AccountManager CreateAccountManager(string manager);
    }
}