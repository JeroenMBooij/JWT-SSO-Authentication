using AuthenticationServer.Common.Models.ContractModels.Token;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface ITokenProcessManager
    {
        Task<JToken> Deserialize(string token);
        Task<bool> ValidateToken(string token);
        Task<Ticket> RefreshToken(Ticket ticket);
    }
}