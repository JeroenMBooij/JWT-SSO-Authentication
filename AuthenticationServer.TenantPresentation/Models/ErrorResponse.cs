using Newtonsoft.Json;
using System.Collections.Generic;

namespace AuthenticationServer.TenantPresentation.Models
{
    //TODO Generate this with NSwag
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; }
    }
}
