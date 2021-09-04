using AuthenticationServer.TenantPresentation.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.TenantPresentation.Pages
{
    public partial class Index
    {
        #region Properties
        [Inject] private IStringLocalizer<Index> Localizer { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        #endregion

        #region Constructors



        #endregion

        #region Public Methods



        #endregion

        #region Private Methods
        private void NavigateToHome()
        {
            NavigationManager.NavigateTo("home");
        }

        private void NavigateToLogin()
        {
            NavigationManager.NavigateTo("login");
        }

        #endregion
    }
}
