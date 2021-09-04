using Authentication.Persistance.DataContext;
using AuthenticationServer.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using System;
using AuthenticationServer.Domain.Entities;
using Microsoft.Extensions.Configuration;

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

                if (isDevelopment)
                    await context.Database.EnsureCreatedAsync();
                else
                {
                    await context.Database.MigrateAsync();

                    IMainSqlDataAccess dataAccess = serviceProvider.GetRequiredService<IMainSqlDataAccess>();
                    string sql = $"INSERT INTO dbo.Languages VALUES (@Id, @Name, @Code, @RfcCode3066, @Created)";
                    var parameters = new 
                    { 
                        Id = Guid.NewGuid().ToString(),
                        Name = "English",
                        Code = "EN",
                        RfcCode3066 = "en-EN",
                        Created = DateTime.UtcNow
                    };
                    await dataAccess.SaveData<LanguageEntity, dynamic>(sql, parameters);

                    sql = $"INSERT INTO dbo.Languages VALUES (@Id, @Name, @Code, @RfcCode3066, @Created)";
                    parameters = new
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Dutch",
                        Code = "NL",
                        RfcCode3066 = "nl-NL",
                        Created = DateTime.UtcNow
                    };

                    await dataAccess.SaveData<LanguageEntity, dynamic>(sql, parameters);
                }
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
                if(!env.Equals("Development"))
                    configuration.AddUserSecrets("6b294752-38ff-4b32-a242-41a1b4ac376d");
            });
    }
}
