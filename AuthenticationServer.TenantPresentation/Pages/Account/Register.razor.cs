using AuthenticationServer.TenantPresentation.Models;
using AuthenticationServer.TenantPresentation.Services.GeneratedClients;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationServer.TenantPresentation.Pages.Account
{
    public partial class Register
    {
        #region Properties



        #endregion
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IStringLocalizer<Login> Localizer { get; set; }
        [Inject] private IAuthenticationClient AuthenticationClient { get; set; }

        [Parameter] public string Email { get; set; }

        //TODO get languages from authentication server
        public enum Languages { English, Dutch }

        private Tenant Tenant { get; set; }
        private int DomainCounter { get; set; }
        private Dictionary<int, DomainName> DomainTracker { get; set; }
        private string RepeatPassword { get; set; }
        public string RegistrationMessage { get; set; }
        public bool RegistrationSuccess { get; set; }

        #region Constructors

        public Register()
        {
            Tenant = new Tenant();
            Tenant.Languages = new List<string>();
            //TODO upload logo
            Tenant.Domains = new List<DomainName>();
            DomainTracker = new Dictionary<int, DomainName>();
            DomainCounter = 1;
            Tenant.UsersJwtConfiguration = new JwtConfiguration();
        }

        #endregion

        #region Methods
        private async Task HandleRegistration()
        {
            if (!Tenant.Password.Equals(RepeatPassword))
            {
                RegistrationMessage = Localizer["PasswordMismatch"];
                return;
            }

            Tenant.Email = Email;
            //TODO Create customizable JSchemas
            TempTenantPropertyBuilder();
            Tenant.Domains.Clear();
            foreach (DomainName domain in DomainTracker.Values)
                Tenant.Domains.Add(domain);

            try
            {
                RegistrationMessage = await AuthenticationClient.TenantAccount_RegisterAsync(Tenant);
                RegistrationSuccess = true;
            }
            catch(AuthenticationApiException exception)
            {
                RegistrationMessage = "";
                string responseString = exception.Response;
                ErrorResponse errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                foreach (ErrorModel error in errorResponse.Errors)
                    RegistrationMessage += error.Message;
            }
        }

        private void TempTenantPropertyBuilder()
        {
            Tenant.DashboardSchema = JSchema.Parse(@"{""temp"": ""test""}");
            Tenant.TrackModelSchema = JSchema.Parse(@"{""temp"": ""test""}");
            Tenant.DataModelSchema = JSchema.Parse(@"{""temp"": ""test""}");
            Tenant.UsersJwtConfiguration.ExpireHours = 6;
        }

        private void AddDomainUrl(int count, string url)
        {
            if (!DomainTracker.ContainsKey(count))
                DomainTracker.Add(count, new DomainName() { Url = url });
            else
            {
                DomainName domain = DomainTracker[count];
                domain.Url = url;
                DomainTracker[count] = domain;
            }
        }

        private void LanguageCheckboxClicked(string selectedLanguage, object @checked)
        {
            if ((bool)@checked)
            {
                if (!Tenant.Languages.Contains(selectedLanguage))
                {
                    Tenant.Languages.Add(selectedLanguage);
                }
            }
            else
            {
                if (Tenant.Languages.Contains(selectedLanguage))
                {
                    Tenant.Languages.Remove(selectedLanguage);
                }
            }
        }

    }

        #endregion

}
