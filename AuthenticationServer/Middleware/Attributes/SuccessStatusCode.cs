using System;

namespace AuthenticationServer.Web.Middleware.Attributes
{
    public class SuccessStatusCode : Attribute
    {
        public SuccessStatusCode(int statusCode)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
    }
}
