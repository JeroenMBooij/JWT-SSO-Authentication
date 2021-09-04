using AuthenticationServer.Logic.Generated;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AuthenticationServer.Logic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLogic(this IServiceCollection services, IConfiguration config)
        {

            services.Scan(scan => scan
                .FromAssemblyOf<ILogicAssembly>()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Manager") || type.Name.EndsWith("Handler") || type.Name.EndsWith("Factory")))
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
