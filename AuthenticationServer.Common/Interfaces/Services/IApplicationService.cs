using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Domain.Entities;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface IApplicationService
    {
        Task<string> RegisterApplicationAsync(ApplicationAccount applicationAccount);
    }
}