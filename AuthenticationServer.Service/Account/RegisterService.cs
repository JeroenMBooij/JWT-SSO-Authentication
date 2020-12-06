using AuthenticationServer.Common.Interfaces.Domain.Repository;
using AuthenticationServer.Common.Interfaces.Logic.JWT;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ResponseModels;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Service
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userDataAccess;
        private readonly IJwtManager _jwtManager;

        public RegisterService(IUserRepository userDataAccess, IJwtManager jwtManager)
        {
            _userDataAccess = userDataAccess;
            _jwtManager = jwtManager;
        }

        public async Task<string> RegisterTenant(Tenant tenant)
        {
            JwtConfiguration model = await _userDataAccess.CreateTenant(tenant);

            try
            {
                return _jwtManager.GenerateToken(model);
            }
            catch (Exception)
            {
                //TODO log error
                //throw new BadRequestException("");
                return null;
            }
        }

        public async Task<string> RegisterUser(User user)
        {
            // TODO Validate VM and automapper
            JwtConfiguration model = await _userDataAccess.CreateUser(user);
            //JwtConfiguration model = GetJWTContainerModel(user);

            return _jwtManager.GenerateToken(model);
        }

        
    }
}
