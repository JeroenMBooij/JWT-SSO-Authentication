using AuthenticationServer.Common.Interfaces.Domain;
using AuthenticationServer.Domain.DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainContext>(options =>
                         options.UseSqlServer(
                             configuration.GetConnectionString("MainConnection")));
            services.AddScoped<ISqlDataAccess, MainSqlDataAccess>();

            services.Scan(scan => scan
                .FromAssemblyOf<ISqlDataAccess>()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Data")))
                .AsMatchingInterface()
                .WithScopedLifetime()
                );

            return services;
        }
    }
}
