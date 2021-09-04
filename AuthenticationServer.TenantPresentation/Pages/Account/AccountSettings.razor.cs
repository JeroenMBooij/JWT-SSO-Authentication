using AuthenticationServer.TenantPresentation.Services.GeneratedClients;
using AuthenticationServer.TenantPresentation.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.TenantPresentation.Pages.Account
{
    public partial class AccountSettings
    {
        #region Properties
        [Inject] private IStringLocalizer<Login> Localizer { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private HostAuthenticationStateProvider HostAuthenticationStateProvider { get; set; }
        private Tenant Tenant { get; set; }
        private DomainName NewDomain { get; set; }
        private string NewDomainErrorMessage { get; set; }

        #endregion

        #region Constructors

        public AccountSettings()
        {
            Tenant = new Tenant();
            Tenant.Languages = new List<string>();
            Tenant.Domains = new List<DomainName>();
            NewDomain = new DomainName();
        
        }

        #endregion

        #region Methods
        protected async override Task OnInitializedAsync()
        {
            Tenant = await HostAuthenticationStateProvider.GetTenantAsync();

            //TODO create API to get this data (do not store this in jwt)
            string token = await HostAuthenticationStateProvider.GetTokenAsync();
            Tenant.Domains = new List<DomainName>();
            Tenant.Domains.Add(new DomainName() { Url = "https://www.google.nl" });
            Tenant.Domains.Add(new DomainName() { Url = "https://www.jmbpowered.nl" });

            await base.OnInitializedAsync();
        }

        private async Task AddNewDomain()
        {

        }
        #endregion

        private void AddDomainLogo(object imageFile)
        {

        }

    }
}
