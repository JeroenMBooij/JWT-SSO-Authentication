using Newtonsoft.Json.Linq;

namespace AuthenticationServer.Common.Interfaces.Services
{
    public interface ITokenProcessService
    {
        JToken Deserialize(string token);
        bool ValidateToken(string token);
    }
}