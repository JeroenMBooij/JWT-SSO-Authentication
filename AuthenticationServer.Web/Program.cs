using Authentication.Persistance.DataContext;
using AuthenticationServer.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace AuthenticationServer.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            #region Production/Staging setup
            using (var serviceScope = host.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                var isDevelopment = serviceProvider.GetRequiredService<IWebHostEnvironment>().IsDevelopment();

                using var context = serviceProvider.GetRequiredService<MainIdentityContext>();

                await context.Database.EnsureCreatedAsync();
                await context.Database.MigrateAsync();

            }
            #endregion

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .CreateInfrastructureBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                var env = hostingContext.HostingEnvironment.EnvironmentName;
                if (!env.Equals("Development"))
                    configuration.AddUserSecrets("6b294752-38ff-4b32-a242-41a1b4ac376d");
            });
    }
}
