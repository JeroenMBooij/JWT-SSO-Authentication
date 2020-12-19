using AuthenticationServer.Common.Models.ContractModels.Common;
using System.Collections.Generic;

namespace AuthenticationServer.Common.Models.ContractModels
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
