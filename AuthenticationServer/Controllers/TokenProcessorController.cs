using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Web.Middleware.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AuthenticationServer.Web.Controllers
{
    [ServiceFilter(typeof(AuthenticateTenantAttribute))]
    [Route("api/[controller]")]
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
        [HttpGet]
        [Route("deserialize/{token}")]
        public JToken DeserialzeToken(string token)
        {
            return _tokenProcessService.Deserialize(token);
        }

        /// <summary>
        /// Give me your token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Validate/{token}")]
        public JToken ValidateToken(string token)
        {
            return _tokenProcessService.IsValid(token);
        }
    }
}
