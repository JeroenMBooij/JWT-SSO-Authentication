using AuthenticationServer.Common.Constants.Token;
using AuthenticationServer.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Extentions
{
    public static class EnumExtensions
    {
        public static string ToSchema(this SecurityAlgorithm securityAlgorithm)
        {
            return SupportedAlgorithms.List.Where(s => s.Name == securityAlgorithm).FirstOrDefault().Schema;
        }
    }
}
