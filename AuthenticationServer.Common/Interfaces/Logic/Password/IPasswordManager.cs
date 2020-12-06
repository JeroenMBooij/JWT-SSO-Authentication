

namespace AuthenticationServer.Common.Interfaces.Logic.Password
{
    public interface IPasswordManager
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}