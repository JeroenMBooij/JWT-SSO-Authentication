using Authentication.Persistance;
using AuthenticationServer.Infrastructure;
using AuthenticationServer.Logic;
using AuthenticationServer.Logic.Generated;
using AuthenticationServer.Logic.Managers;
using AuthenticationServer.Service;
using AuthenticationServer.Web.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region endpoints
            services.AddMyEndpoints();
            services.AddMySwagger(Configuration);
            #endregion

            #region Clean Dependency Injection
            services.AddLogic(Configuration);
            services.AddServices();
            services.AddPersistance(Configuration);
            services.AddInfrastructure(Configuration);
            #endregion

            #region Generated Http Clients
            services.AddHttpClient<IEmailClient, EmailClient>(configuration => configuration.BaseAddress = new Uri(Configuration["BaseUrls:EmailServer"]));
            #endregion

            #region Authentication
            var jwtManager = new JwtManager("startup", Configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = jwtManager.GetTokenValidationParameters();

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
            //app.UseHttpsRedirection();
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
