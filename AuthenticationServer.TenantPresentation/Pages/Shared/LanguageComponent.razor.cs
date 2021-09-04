using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;

namespace AuthenticationServer.TenantPresentation.Pages.Shared
{
    public partial class LanguageComponent
    {
        #region Properties
        [Inject] public NavigationManager navigationManager { get; set; }
        [Inject] public IStringLocalizer<MainLayout> Localizer { get; set; }

        private string currentCulture = Thread.CurrentThread.CurrentCulture.Name;

        #endregion

        #region Constructors



        #endregion

        #region Public Methods


        #endregion

        #region Private Methods

        private void SelectCulture(string culture)
        {
            if (culture == currentCulture)
                return;

            var uri = new Uri(navigationManager.Uri)
                .GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);

            var query = $"?culture={Uri.EscapeDataString(culture)}&" +
                $"redirectionUri={Uri.EscapeDataString(uri)}";

            navigationManager.NavigateTo("Culture/SetCulture" + query, forceLoad: true);
        }

        #endregion
    }
}
