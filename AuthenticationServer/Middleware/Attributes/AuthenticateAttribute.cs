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
        private readonly IJwtManager _jwtManager;
        private readonly ITenantRepository _tenantRepository;

        public AuthenticateAttribute(IJwtManager jwtManager, ITenantRepository tenantRepository)
        {
            _jwtManager = jwtManager;
            _tenantRepository = tenantRepository;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var tenantToken = filterContext.HttpContext.Request.Cookies["tenant-authorization-token"];
            if (string.IsNullOrEmpty(tenantToken))
            {
                var errorResponse = new ErrorResponse();
                errorResponse.Errors.Add(new ErrorModel() { FieldName = "tenant-authorization-token cookie", Message = "Token is empty." });


                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            if (!_jwtManager.IsTokenValid(tenantToken))
            {
                var errorResponse = new ErrorResponse();
                errorResponse.Errors.Add(new ErrorModel() { FieldName = "tenant-authorization-token cookie", Message = "Token is invalid" });

                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            //TODO Validate URL belongs to Tenant

            base.OnActionExecuting(filterContext);
        }
    }
}
