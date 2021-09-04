using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Domain.Entities;
using AuthenticationServer.Web.Middleware.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IJwtManager _jwtManager;
        private readonly IApplicationService _applicationService;

        public ApplicationController(IJwtManager jwtManager, IApplicationService applicationService)
        {
            _jwtManager = jwtManager;
            _applicationService = applicationService;
        }

        /// <summary>
        /// Register an application to manage your users
        /// </summary>
        /// <param name="applicationAccount"></param>
        /// <returns>validation message</returns>
        [HttpPost]
        [Route("Register")]
        [SuccessStatusCode(StatusCodes.Status201Created)]
        public async Task<string> Register([FromBody] ApplicationAccount applicationAccount)
        {
            return await _applicationService.RegisterApplicationAsync(applicationAccount);
        }
    }
}
