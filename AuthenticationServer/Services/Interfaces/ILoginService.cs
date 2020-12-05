using Newtonsoft.Json.Linq;

namespace AuthenticationServer.Services.Interfaces
{
    public interface ILoginService
    {
        bool IsValid(string token);
        JToken Deserialize(string token);

    }
}