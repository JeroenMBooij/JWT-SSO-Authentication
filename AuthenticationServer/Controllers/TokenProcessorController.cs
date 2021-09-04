using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Web.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AuthenticationServer.Web.Controllers
{
    [ServiceFilter(typeof(AuthenticateAttribute))]
    [Route("[controller]")]
    [ApiController]
    public class TokenProcessorController : ControllerBase
    {
        private readonly ITokenProcessService _tokenProcessService;

        public TokenProcessorController(ITokenProcessService tokenProcessService)
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
        public JToken DeserialzeToken([FromBody] string token)
        {
            return _tokenProcessService.Deserialize(token);
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
