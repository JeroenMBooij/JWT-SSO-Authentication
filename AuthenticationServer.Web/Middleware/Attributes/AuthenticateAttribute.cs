using AuthenticationServer.Common.Interfaces.Domain.Repositories;
using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;


 // ############## IS CURRENTLY NOT IN USE ANYMORE #################
 // We might want to use this again if we want to do some fancy tricks inside the authentication pipeline

namespace AuthenticationServer.Web.Middleware.Attributes
{
    public class AuthenticateAttribute : ActionFilterAttribute
    {
        private readonly IJwtTokenWorker _jwtManager;
        private readonly ITenantAccountRepository _tenantRepository;
        private readonly IAdminAccountRepository _applicationRepository;

        public AuthenticateAttribute(IJwtTokenWorker jwtManager, ITenantAccountRepository tenantRepository, IAdminAccountRepository applicationRepository)
        {
            _jwtManager = jwtManager;
            _tenantRepository = tenantRepository;
            _applicationRepository = applicationRepository;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var tenantToken = filterContext.HttpContext.Request.Headers["authorization"];
            if (string.IsNullOrEmpty(tenantToken))
            {
                var errorResponse = new ErrorResponse();
                errorResponse.Errors.Add(new ErrorModel() { FieldName = "authorization", Message = "Token is empty." });


                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            if (!_jwtManager.IsTokenValid(null, tenantToken))
            {
                var errorResponse = new ErrorResponse();
                errorResponse.Errors.Add(new ErrorModel() { FieldName = "authorization", Message = "Token is invalid" });

                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
