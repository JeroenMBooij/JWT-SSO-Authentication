using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Web.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AuthenticationServer.Web.Controllers
{
    [ServiceFilter(typeof(AuthenticateAttribute))]
    [Route("[controller]")]
    [ApiController]
    public class TenantTokenController : ControllerBase
    {
        private readonly ITokenProcessService _tokenProcessService;

        public TenantTokenController(ITokenProcessService tokenProcessService)
        {
            _tokenProcessService = tokenProcessService;
        }
        /// <summary>
        /// Give me your token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("deserialize")]
        public string DeserialzeToken([FromBody] string token)
        {
            return _tokenProcessService.Deserialize(token).ToString();
        }

        /// <summary>
        /// Give me your token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Validate")]
        public bool ValidateToken([FromBody] string token)
        {
            return _tokenProcessService.ValidateToken(token);
        }
    }
}
