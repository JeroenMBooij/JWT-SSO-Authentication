using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationServer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Auto Mapper
            services.AddAutoMapper(config =>
            {
                config.RecognizeDestinationPrefixes(new[] { "Tenant", "User", "Role", "Domain", "Language", "Dashboard" });
                config.RecognizePrefixes(new[] { "Tenant", "User", "Role", "Domain", "Language", "Dashboard" });

            }, typeof(DependencyInjection));
            #endregion

            return services;
        }
    }
}
