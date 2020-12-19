using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
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

            services.AddScoped<IMainSqlDataAccess, MainSqlDataAccess>();

            services.Scan(scan => scan
                .FromAssemblyOf<IPersistanceAssembly>()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
                .AsMatchingInterface()
                .WithScopedLifetime()
            );

            return services;
        }
    }

    public interface IPersistanceAssembly
    {

    }
}
