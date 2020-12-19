using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Common.Models.ContractModels;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface IUserAccountService
    {
        Task<string> RegisterUserAsync(User user, string url);
    }
}