using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                .AsMatchingInterface()
                .WithScopedLifetime()
                );

            return services;
        }
    }
}
