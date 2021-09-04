using AuthenticationServer.TenantPresentation.Services.GeneratedClients;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace AuthenticationServer.TenantPresentation.Pages.Account
{
    public partial class RecoverPassword
    {
        #region Properties
        [Inject] private IAuthenticationClient AuthenticationClient { get; set; }
        [Inject] private IStringLocalizer<Login> Localizer { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }


        #endregion

        #region Constructors



        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}
