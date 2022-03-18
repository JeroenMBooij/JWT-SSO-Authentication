using Authentication.Persistance;
using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Generated;
using AuthenticationServer.Common.Models.DTOs;
using AuthenticationServer.Logic;
using AuthenticationServer.Logic.Workers;
using AuthenticationServer.Services;
using AuthenticationServer.Web.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Web
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
                builder.AddEventSourceLogger();
            });
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }
        private readonly ILogger<Startup> _logger;

        public void ConfigureServices(IServiceCollection services)
        {
            #region endpoints
            services.AddLogging();
            services.AddMyEndpoints(); 
            services.AddMySwagger(Configuration);
            #endregion

            #region Clean Dependency Injection
            services.AddLogic(Configuration);
            services.AddPersistance(Configuration);
            services.AddInfrastructure(Configuration);
            #endregion

            #region Generated Http Clients
            services.AddHttpClient<IEmailClient, EmailClient>(configuration => configuration.BaseAddress = new Uri(Configuration["BaseUrls:EmailServer"]));
            #endregion

            #region Authentication
            var jwtManager = new JwtTokenWorker("startup", Configuration, _logger);
            var jwtconfig = new JwtConfig()
            {
                SecretKey = Configuration["JWT_SECRETKEY"],
                ExpireMinutes = double.Parse(Configuration["JwtAdminConfig:ExpireMinutes"]),
                Algorithm = Enum.Parse<SecurityAlgorithm>(Configuration["JwtAdminConfig:Algorithm"]),
                ValidateIssuer = true,
                Issuer = Configuration["JWT_ISSUER"]
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = jwtManager.GetTokenValidationParameters(jwtconfig);

                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        string authorization = context.Request.Headers["Authorization"];

                        if (string.IsNullOrEmpty(authorization))
                        {
                            context.NoResult();
                            return Task.CompletedTask;
                        }

                        context.Token = authorization.Trim();

                        return Task.CompletedTask;
                    }
                };
            });
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            #region routing

            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .Build();
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
            #endregion

            #region configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json",
                             optional: false,
                             reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            #endregion

            #region swagger
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(configure =>
            {
                configure.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                configure.RoutePrefix = string.Empty;
            });
            #endregion

        }
    }

    public interface IWebAssembly
    {

    }
}
