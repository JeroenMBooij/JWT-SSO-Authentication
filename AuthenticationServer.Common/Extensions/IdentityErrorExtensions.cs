using AuthenticationServer.Common.Models.ContractModels.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Extentions
{
    public static class IdentityErrorExtensions
    {
        public static List<ErrorModel> ToErrorModel(this IEnumerable<IdentityError> identityErrors)
        {
            var errorModel = new List<ErrorModel>();

            foreach (IdentityError error in identityErrors)
            {
                errorModel.Add(new ErrorModel() { FieldName = error.Code, Message = error.Description });
            }

            return errorModel;
        }
    }
}
