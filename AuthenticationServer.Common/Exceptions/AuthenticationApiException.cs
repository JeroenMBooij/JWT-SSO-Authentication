using AuthenticationServer.Common.Models.ContractModels.Account;
using System;
using System.Collections.Generic;

namespace AuthenticationServer.Common.Exceptions
{
    public class AuthenticationApiException : Exception
    {
        public string Field { get; }
        public int StatusCode { get; }
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();

        public AuthenticationApiException(string field, string message, int statusCode = 400)
            : base(message)
        {
            StatusCode = statusCode;
            Errors.Add(new ErrorModel() { FieldName = field, Message = message });
        }

        public AuthenticationApiException(List<ErrorModel> errors, int statusCode = 400)
            : base("")
        {
            Errors = errors;
            StatusCode = statusCode;
        }

    }
}
