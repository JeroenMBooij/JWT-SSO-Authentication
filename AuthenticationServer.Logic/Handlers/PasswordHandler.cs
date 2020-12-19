using AuthenticationServer.Common.Interfaces.Logic.Handlers;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace AuthenticationServer.Logic.Handler
{
    public class PasswordHandler : IPasswordHandler
    {
        private readonly IConfiguration _config;
        private readonly string _salt;

        public PasswordHandler(IConfiguration config)
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
