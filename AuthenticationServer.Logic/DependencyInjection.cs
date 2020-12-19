using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationServer.Logic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<ILogicAssembly>()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Manager") ||
                                                             type.Name.EndsWith("Handler")))
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
