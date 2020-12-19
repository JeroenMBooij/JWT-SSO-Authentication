using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Common.Models.ContractModels;
using System.Threading.Tasks;
using AutoMapper;

namespace AuthenticationServer.Service.Account
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IJwtManager _jwtManager;
        private readonly IMapper _mapper;
        private readonly IUserAccountManager _userAccountManager;

        public UserAccountService(IUserAccountManager accountManager, IJwtManager jwtManager, IMapper mapper)
        {
            _jwtManager = jwtManager;
            _mapper = mapper;
            _userAccountManager = accountManager;
        }



        public async Task<string> RegisterUserAsync(User user, string url)
        {
            JwtConfigurationDto model = await _userAccountManager.CreateAccountAsync(_mapper.Map<UserDto>(user), url);

            return _jwtManager.GenerateToken(model);
        }


    }
}
