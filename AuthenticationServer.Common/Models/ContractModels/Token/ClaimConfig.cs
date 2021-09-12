using AuthenticationServer.Common.Constants.Token;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Common.Models.ContractModels
{
    public class ClaimConfig
    {

        [Required]
        public string JwtName { get; set; }

        [Required]
        public ClaimType Type { get; set; }


        /// <summary>
        /// JwtName: claim, Description: AbstractData coolumn name
        /// JwtName: email, Description: ApplicationUser.Email
        /// JwtName: exp = Description: Jwt.exp
        /// JwtName: iat = Description: Jwt.iat
        /// </summary>
        public string Data { get; set; }

        public List<ClaimConfig> ClaimConfigurations { get; set; }
    }
}
