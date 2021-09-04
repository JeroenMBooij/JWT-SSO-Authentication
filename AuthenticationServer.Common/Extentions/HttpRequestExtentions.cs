using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationServer.Common.Extentions
{
    public static class HttpRequestExtentions
    {
        public static string GetDomainUrl(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host.Value}";
        }
    }
}
