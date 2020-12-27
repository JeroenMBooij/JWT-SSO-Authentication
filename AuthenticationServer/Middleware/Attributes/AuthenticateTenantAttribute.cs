using AuthenticationServer.Common.Interfaces.Logic.Managers;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthenticationServer.Web.Middleware.Attributes
{
    public class AuthenticateTenantAttribute : ActionFilterAttribute
    {
        private readonly IJwtManager _jwtManager;

        public AuthenticateTenantAttribute(IJwtManager jwtManager)
        {
            _jwtManager = jwtManager;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var tenantToken = filterContext.HttpContext.Request.Headers["tenant-authorization"];
            if (string.IsNullOrEmpty(tenantToken))
            {
                var errorResponse = new ErrorResponse();
                errorResponse.Errors.Add(new ErrorModel() { FieldName = "tenant-authorization header", Message = "Token is empty." });
                filterContext.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            if (!_jwtManager.IsTokenValid(tenantToken))
            {
                var errorResponse = new ErrorResponse();
                errorResponse.Errors.Add(new ErrorModel() { FieldName = "tenant-authorization header", Message = "Invalid Token" });
                filterContext.Result = new BadRequestObjectResult(errorResponse);
                return;
            }
            else
                base.OnActionExecuting(filterContext);
        }
    }
}
