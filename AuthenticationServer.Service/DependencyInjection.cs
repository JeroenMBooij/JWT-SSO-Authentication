using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationServer.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<IServiceAssembly>()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                .AsMatchingInterface()
                .WithScopedLifetime()
            );

            return services;
        }
    }

    public interface IServiceAssembly
    {

    }
}
