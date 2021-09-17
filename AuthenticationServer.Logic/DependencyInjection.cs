using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationServer.Logic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLogic(this IServiceCollection services, IConfiguration config)
        {

            services.Scan(scan => scan
                .FromAssemblyOf<ILogicAssembly>()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Manager") || type.Name.EndsWith("Worker") || type.Name.EndsWith("Factory")))
                .AsMatchingInterface()
                .WithScopedLifetime()
            );

            return services;
        }
    }

    public interface ILogicAssembly
    {

    }
}
