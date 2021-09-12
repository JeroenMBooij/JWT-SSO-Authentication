using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.ContractModels.Applications;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface IAdminAccountService : IAccountService
    {
        Task<string> LoginAsync(Credentials credentials);
        Task<string> RegisterAsync(AdminAccount applicationAccount);
        Task UpdateApplicationAsync(ApplicationWithId application);
    }
}