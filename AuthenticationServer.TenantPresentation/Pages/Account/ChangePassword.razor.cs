using AuthenticationServer.TenantPresentation.Models;
using AuthenticationServer.TenantPresentation.Services.GeneratedClients;
using AuthenticationServer.TenantPresentation.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationServer.TenantPresentation.Pages.Account
{
    public partial class ChangePassword
    {
        #region Properties
        [Inject] private IStringLocalizer<Login> Localizer { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IAuthenticationClient AuthenticationClient { get; set; }
        [Inject] private HostAuthenticationStateProvider HostAuthenticationStateProvider { get; set; }


        private NewCredentials NewCredentials { get; set; }
        private string RepeatNewPassword { get; set; }
        private string Message { get; set; }
        private bool Changed { get; set; }

        #endregion

        #region Constructors
        public ChangePassword()
        {
            NewCredentials = new NewCredentials();
        }

        protected async override Task OnInitializedAsync()
        {
            NewCredentials.Email = (await HostAuthenticationStateProvider.GetTenantAsync()).Email;

            await base.OnInitializedAsync();
        }

        #endregion

        #region Methods
        private async Task HandleChangePassword()
        {
            if (NewCredentials.NewPassword.Equals(RepeatNewPassword))
            {
                try
                {
                    await AuthenticationClient.TenantAccount_ChangePasswordAsync(NewCredentials);
                    Message = "Your password has been changed successfully.";
                    Changed = true;
                }
                catch (AuthenticationApiException exception)
                {
                    Message = "";
                    string responseString = exception.Response;
                    ErrorResponse errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    foreach (ErrorModel error in errorResponse.Errors)
                        Message += error.Message;
                }
            }
        }

        #endregion

    }
}
