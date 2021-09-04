

using AuthenticationServer.TenantPresentation.Services.GeneratedClients;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.TenantPresentation.Pages.Shared
{
    public partial class MainLayout
    {
        public Tenant Tenant { get; set; }

        public MainLayout()
        {

        }


        

        private Task<string> GetTokenAsync()
        {
            throw new NotImplementedException();
        }
    }
}
