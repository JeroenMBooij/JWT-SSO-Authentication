using AuthenticationServer.Common.Interfaces.Logic.Password;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace AuthenticationServer.Logic.Password
{
    public class PasswordManager : IPasswordManager
    {
        private readonly IConfiguration _config;
        private readonly string _salt;

        public PasswordManager(IConfiguration config)
        {
            _config = config;
            _salt = _config.GetValue<string>("salt");
        }
        public string HashPassword(string password)
        {
            SaltPassword(ref password);
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, hashType: HashType.SHA384);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            SaltPassword(ref password);
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword, hashType: HashType.SHA384);
        }

        private void SaltPassword(ref string password)
        {
            password.Concat(_salt).ToString();
        }
    }
}
