using AuthenticationServer.TenantPresentation.Models;
using AuthenticationServer.TenantPresentation.Services.GeneratedClients;
using AuthenticationServer.TenantPresentation.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationServer.TenantPresentation.Pages.Account
{
    public partial class Login
    {
        #region Properties
        [Inject] private IStringLocalizer<Login> Localizer { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private HostAuthenticationStateProvider HostAuthenticationStateProvider { get; set; }

        public bool Registered { get; set; }
        public Credentials Credentials { get; set; }
        public string LoginErrorMessage { get; set; }

        public string NewEmail { get; set; } 
        #endregion

        #region Constructors
        public Login()
        {
            Registered = true;
            Credentials = new Credentials();
            NewEmail = "";
        }
        #endregion

        #region Methods
        private async Task HandleLogin()
        {

            var stringContent = new StringContent(JsonSerializer.Serialize(Credentials), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PostAsync($"{NavigationManager.BaseUri}Auth/Login", stringContent);

            //TODO handle error and httpclient properly
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                ErrorResponse errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                LoginErrorMessage = "";
                foreach (ErrorModel error in errorResponse.Errors)
                    LoginErrorMessage += error.Message;

                return;
            }

            string token = await response.Content.ReadAsStringAsync();
            await HostAuthenticationStateProvider.SetTokenAsync(token);

            NavigationManager.NavigateTo("/home");

        }

        private void HandleRegister()
        {
            Registered = false;
        }

        #endregion
    }
}
