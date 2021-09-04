using AuthenticationServer.TenantPresentation.Services.GeneratedClients;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationServer.TenantPresentation.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly IAuthenticationClient _authenticationClient;

        public AuthController(IAuthenticationClient authenticationClient)
        {
            _authenticationClient = authenticationClient;
        }
        public async Task<string> Login([FromBody] Credentials credentials)
        {
            try
            {
                string token = await _authenticationClient.TenantAccount_LoginAsync(credentials);

                return token;
            }
            catch (AuthenticationApiException exception)
            {
                Response.StatusCode = exception.StatusCode;
                return exception.Response;
            }

        }
    }
}
