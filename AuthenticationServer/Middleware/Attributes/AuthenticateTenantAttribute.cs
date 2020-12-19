using AuthenticationServer.Common.Interfaces.Logic.Managers;
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
            var jwt = _jwtManager;
            
            var test = filterContext.HttpContext.Request.Headers["tenant-authorization"];
            var host = filterContext.HttpContext.Request.Host.Value;
            base.OnActionExecuting(filterContext);
        }
    }
}
