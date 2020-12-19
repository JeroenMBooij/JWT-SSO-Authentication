

namespace AuthenticationServer.Common.Interfaces.Logic.Handlers
{
    public interface IPasswordHandler
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}