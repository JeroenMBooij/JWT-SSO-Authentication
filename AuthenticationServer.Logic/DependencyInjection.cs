using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Logic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Manager")))
                .AsMatchingInterface()
                .WithScopedLifetime()
                );

            return services;
        }
    }
}
