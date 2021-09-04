using AuthenticationServer.TenantPresentation.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.TenantPresentation.Pages.Shared
{
    public partial class NavMenu
    {
        [Parameter] public Action<string> SetHeaderEvent { get; set; }
        [Inject] private IStringLocalizer<MainLayout> Localizer { get; set; }
        [Inject] private HostAuthenticationStateProvider HostAuthenticationStateProvider { get; set; }

        private bool collapseNavMenu = true;
        private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void SetHeader(string name)
        {
            SetHeaderEvent(name);
        }

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        private async Task LogOut()
        {
            await HostAuthenticationStateProvider.SetTokenAsync(null);
        }

    }
}
