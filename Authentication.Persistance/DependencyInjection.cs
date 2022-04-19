using Authentication.Persistance.DataContext;
using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using AuthenticationServer.Domain.DataAccess.DataContext;
using AuthenticationServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;

namespace Authentication.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            #region Data Access
            var connectionString = GetDatabaseConnectionString(configuration);
            var retry = Policy.Handle<Exception>()
                          .WaitAndRetry(
                              retryCount: 10,
                              // 2,4,8,16,32 etc.
                              sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            retry.Execute(() =>
            {
                services.AddDbContext<DbContext>(options =>
                {
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                });
            });

            services.AddSingleton<IMainSqlDataAccess, MainSqlDataAccess>();

            #endregion

            #region Repositories
            services.Scan(scan => scan
                .FromAssemblyOf<IPersistanceAssembly>()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
                .AsMatchingInterface()
                .WithScopedLifetime()
            );
            #endregion

            #region Tenant Identity

            services.AddIdentity<ApplicationUserEntity, RoleEntity>();
            services.AddIdentityCore<ApplicationUserEntity>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
                 .AddRoles<RoleEntity>()
                 .AddEntityFrameworkStores<MainIdentityContext>()
                 .AddDefaultTokenProviders();


            #endregion


            return services;
        }
        public static string GetDatabaseConnectionString(IConfiguration configuration)
        {
            var dbHost = configuration["DB_HOST"];
            var dbName = configuration["DB_NAME"];
            var dbUser = configuration["DB_USER"];
            var dbPassword = configuration["DB_PASSWORD"];

            return $"Server={dbHost}; Database={dbName}; Uid={dbUser}; Pwd={dbPassword}";
        }
    }

    public interface IPersistanceAssembly
    {

    }
}
