using AuthenticationServer.Common.Constants.Token;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Controllers
{
    [Route("api/tenant/token", Name = "Token Processor")]
    [ApiController]
    public class TenantTokenController : ControllerBase
    {
        private readonly ITokenProcessManager _tokenProcessService;

        public TenantTokenController(ITokenProcessManager tokenProcessService)
        {
            _tokenProcessService = tokenProcessService;
        }


        /// <param name="type">filter by algorithm type (autocompleted)</param>
        [HttpGet("algorithms")]
        public dynamic GetSecurityDescriptions(string type = "")
        {
            var algorithms = SupportedAlgorithms.List.Select(s => (dynamic)new
            {
                Name = s.Name.ToString(),
                Schema = s.Schema,
                Type = s.Type.ToString()
            }).ToList();

            if (string.IsNullOrEmpty(type) == false)
                algorithms = algorithms.Where(s => s.Type.StartsWith(type, StringComparison.InvariantCultureIgnoreCase)).ToList();

            return new
            {
                Total = algorithms.Count,
                items = algorithms
            };
        }



        /// <summary>
        /// Give me your token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("deserialize")]
        public async Task<string> DeserialzeToken([FromBody] string token)
        {
            return (await _tokenProcessService.Deserialize(token)).ToString();
        }

        /// <summary>
        /// Give me your token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("validate")]
        public async Task<bool> ValidateToken([FromBody] string token)
        {
            return await _tokenProcessService.ValidateToken(token);
        }

        /// <summary>
        /// Give me your ticket
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<Ticket> RefreshToken([FromBody] Ticket ticket)
        {
            Ticket refreshTicket = await _tokenProcessService.RefreshToken(ticket);
            Response.Cookies.Append(
                "authorization",
                JsonSerializer.Serialize(refreshTicket),
                new CookieOptions()
                {
                    Path = "/"
                });

            return refreshTicket;
        }
    }
}
