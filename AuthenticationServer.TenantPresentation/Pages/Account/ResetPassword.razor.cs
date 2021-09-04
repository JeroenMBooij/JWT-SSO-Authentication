using AuthenticationServer.TenantPresentation.Models;
using AuthenticationServer.TenantPresentation.Services.GeneratedClients;
using AuthenticationServer.TenantPresentation.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationServer.TenantPresentation.Pages.Account
{
    public partial class ResetPassword
    {
        #region Properties
        [Inject] private IAuthenticationClient AuthenticationClient { get; set; }
        [Inject] private IStringLocalizer<Login> Localizer { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private string Email { get; set; }
        private bool ResetToken { get; set; }
        private string Message { get; set; }
        #endregion

        #region Constructors
        public ResetPassword()
        {
            Email = "";
        }

        #endregion

        #region Methods
        private async Task HandlePasswordReset()
        {
            try
            {
                await AuthenticationClient.TenantAccount_ResetPasswordAsync(Email);
                Message = $"We send an email to {Email} to change your password.";
                ResetToken = true;
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


        #endregion

    }
}
