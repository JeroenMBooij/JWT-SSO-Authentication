using Authentication.Logic;
using Authentication.Persistance;
using Authentication.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace AuthenticationServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region endpoints
            services.AddControllers();
            services.AddMvc().AddNewtonsoftJson();
            #endregion

            #region Swagger
            // Set the comments path for the Swagger JSON and UI.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Jiren's Authentication Server",
                    Description = "An API to handle all your application authentication and authorization needs. " +
                                    "By integrating this service you can monitor everything your users do. To start using this service, become a Tenant now!",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Jeroen Booij",
                        Email = "jmbooij.a@gmail.com",
                        Url = new Uri("https://www.facebook.com/people/Jeroen-Booij/100018633216320"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                c.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });
            #endregion

            #region Clean Dependency Injection
            services.AddPersistance(Configuration);
            services.AddServices();
            services.AddLogic();
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            #region routing
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #endregion
        }
    }
}
