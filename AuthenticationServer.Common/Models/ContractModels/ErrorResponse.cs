using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationServer.Common.Models.ContractModels
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
