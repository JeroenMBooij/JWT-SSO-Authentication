using System.Collections.Generic;

namespace AuthenticationServer.Common.Models.ResponseModels
{
    public class Domain
    {
        public string URL { get; set; }
        public List<User> User { get; set; }
        public Tenant Tenant { get; set; }
    }
}
