using Authentication.Persistance;
using AuthenticationServer.Infrastructure;
using AuthenticationServer.Logic;
using AuthenticationServer.Service;
using AuthenticationServer.Web.Middleware;
using AuthenticationServer.Web.Middleware.Attributes;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddControllers();

            services.AddMySwagger();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .Build());
            });

            services.AddMvc()
                .AddFluentValidation(mvcConfig => mvcConfig.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddNewtonsoftJson();

            #region Controller Attributes
            services.Scan(scan => scan
                .FromAssemblyOf<IWebAssembly>()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Attribute")))
                .AsSelf()
                .WithScopedLifetime()
            );
            #endregion

            #endregion


            #region Clean Dependency Injection
            services.AddLogic();
            services.AddServices();
            services.AddPersistance(Configuration);
            services.AddInfrastructure(Configuration);
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            #region routing
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseAuthentication();
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
                app.UseDeveloperExceptionPage();
                builder.AddUserSecrets<Startup>();
            }
            #endregion

            #region swagger
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            #endregion


        }
    }

    public interface IWebAssembly
    {

    }
}
