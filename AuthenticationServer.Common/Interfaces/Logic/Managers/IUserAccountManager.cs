
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Common.Models.ContractModels;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Logic.Managers
{
    public interface IUserAccountManager
    {
        Task<JwtConfigurationDto> CreateAccountAsync(UserDto account, string url);
        Task<JwtConfigurationDto> GetUserJwtConfigurationAsync(UserDto userDto);
    }
}
