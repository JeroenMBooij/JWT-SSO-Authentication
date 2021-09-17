using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels.Applications;
using AuthenticationServer.Web.Middleware.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationServer.Web.Controllers
{
    [Route("api/[controller]", Name = "Applications")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationManager _applicationService;

        public ApplicationController(IApplicationManager applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<List<ApplicationWithId>> GetAll()
        {
            string token = Request.Headers["Authorization"].ToString();

            // FIX seperation by multi part entities by Dapper
            return await _applicationService.GetApplications(token);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ApplicationWithId> Get(Guid id)
        {
            string token = Request.Headers["Authorization"].ToString();

            return await _applicationService.GetApplication(token, id);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [SwaggerOperation(OperationId = "POST_API")]
        [SuccessStatusCode(StatusCodes.Status201Created)]
        public async Task<string> Post([FromBody] Application application)
        {
            string token = Request.Headers["Authorization"].ToString();

            return (await _applicationService.CreateApplication(token, application)).ToString();
        }



        /// <remarks>
        /// This endpoint will merge with your existing application and only override the properties you provide
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [SuccessStatusCode(StatusCodes.Status204NoContent)]
        public async Task Put(Guid id, [FromBody] Application application)
        {
            string token = Request.Headers["Authorization"].ToString();

            // TODO FIX Return status codes
            await _applicationService.UpdateApplication(token, id, application);
        }


        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [SuccessStatusCode(StatusCodes.Status204NoContent)]
        public async Task Delete(Guid id)
        {
            string token = Request.Headers["Authorization"].ToString();

            await _applicationService.DeleteApplication(token, id);

        }
    }
}
