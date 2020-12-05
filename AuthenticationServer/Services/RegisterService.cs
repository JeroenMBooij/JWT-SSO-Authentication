using AuthenticationServer.DataAccess;
using AuthenticationServer.DataAccess.Interfaces;
using AuthenticationServer.Logic.Managers;
using AuthenticationServer.Logic.Managers.Interfaces;
using AuthenticationServer.Logic.Models;
using AuthenticationServer.Logic.Models.Interfaces;
using AuthenticationServer.Models;
using AuthenticationServer.Models.ViewModels;
using AuthenticationServer.Services.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationServer.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserData _userDataAccess;
        private readonly IConfiguration _config;

        public RegisterService(IUserData userDataAccess, IConfiguration config)
        {
            _userDataAccess = userDataAccess;
            _config = config;
        }
        public string RegisterUser(UserVM user)
        {
            // TODO Validate VM
            _userDataAccess.Insert(user);

            IAuthContainerModel model = GetJWTContainerModel(user.Name, user.Email);
            IAuthManager authManager = new JWTManager(model.SecretKey);

            return authManager.GenerateToken(model);
        }

        private JWTContainerModel GetJWTContainerModel(string name, string email)
        {
            JWTContainerModel jwtContainerModel = _config.GetSection("JWTSecurity").Get<JWTContainerModel>();
            jwtContainerModel.Claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email)
            };

            return jwtContainerModel;
        }

    }
}
