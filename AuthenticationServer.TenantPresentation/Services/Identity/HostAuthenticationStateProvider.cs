using AuthenticationServer.TenantPresentation.Services.GeneratedClients;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationServer.TenantPresentation.Services.Identity
{
    public class HostAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IWebHostEnvironment _env;
        private readonly IJSRuntime _jsRuntime;
        private readonly IAuthenticationClient _authenticationClient;

        public async Task<string> GetTokenAsync() => await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jeroensso-jwt");

        public async Task SetTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "jeroensso-jwt");

            else
                await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "jeroensso-jwt", token);


            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }


        public HostAuthenticationStateProvider(IWebHostEnvironment env, IJSRuntime jsRuntime, IAuthenticationClient authenticationClient)
        {
            _env = env;
            _jsRuntime = jsRuntime;
            _authenticationClient = authenticationClient;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = "";
            try
            {
                token = await GetTokenAsync();
                if (!string.IsNullOrEmpty(token))
                    await _authenticationClient.TenantAccount_AuthenticationStateAsync(token);
            }
            catch (AuthenticationApiException exception)
            {
                // Token is expired
                await SetTokenAsync(null);
            }
            catch (Exception)
            {
                await SetTokenAsync(null);
                if (!_env.IsDevelopment())
                    throw new Exception("The Authentication Server is currently down.");
            }

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                var claimsIdentity = new ClaimsIdentity(jsonToken.Claims, "Custom");

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return new AuthenticationState(claimsPrincipal);
            }
            else
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        //TODO cache tenant by token key
        public async Task<Tenant> GetTenantAsync()
        {
            var tenant = new Tenant();
            string token = await GetTokenAsync();
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            try
            {
                tenant.Email = jsonToken.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Email).FirstOrDefault().Value;
                //TODO remove middlename add company name
                tenant.Firstname = jsonToken.Claims.Where(x => x.Type == JwtRegisteredClaimNames.GivenName).FirstOrDefault().Value;
                tenant.Lastname = jsonToken.Claims.Where(x => x.Type == JwtRegisteredClaimNames.FamilyName).FirstOrDefault().Value;
                //TODO set supported languages in web api
                tenant.Languages = JsonConvert.DeserializeObject<List<string>>(jsonToken.Claims.Where(x => x.Type == "SupportedLanguages").FirstOrDefault().Value);
            }
            catch (Exception)
            {
                await SetTokenAsync(null);
                //TODO create a global modal popup
                throw new Exception("Something went wrong with your identity. Please log in again.");
            }

            return tenant;
        }
    }
}
