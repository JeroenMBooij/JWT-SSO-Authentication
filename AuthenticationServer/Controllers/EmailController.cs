using AuthenticationServer.Common.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif  
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        [Route("VerifyEmail/{code}")]
        public async Task<string> VerifyEmail(string code)
        {
            await _emailService.VerifyTenantEmail(code);

            return "verfied";
            //return new ContentResult
            //{
            //    ContentType = "text/html",
            //    Content = "<div>Hello World</div>"
            //};
        }
    }
}
