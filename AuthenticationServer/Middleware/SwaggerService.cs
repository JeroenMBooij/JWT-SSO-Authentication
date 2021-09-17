using AuthenticationServer.Web.Middleware.Filters.Swagger;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Reflection;

namespace AuthenticationServer.Web.Middleware
{
    public static class SwaggerService
    {
        public static IServiceCollection AddMySwagger(this IServiceCollection services, IConfiguration config)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, SwaggerProducesFilter>());

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Jeroen's Authentication Server",
                    Description = $@"An API to handle all your application authentication needs with JWT.
                                    By integrating this service you can simply configure your JWT and userdata. 
                                    Start by registering an Admin account and an application then simply register your users as tenants to your application with the same endpoint",
                    Contact = new OpenApiContact
                    {
                        Name = "Jeroen Booij",
                        Email = "jmbooij.a@gmail.com",
                        Url = new Uri("https://twitter.com/Jeroen57971625"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under The MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    }
                });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                options.ExampleFilters();
                options.OperationFilter<SwaggerFileOperationFilter>();
                options.OperationFilter<SwaggerAuthOperationFilter>();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();

            return services;
        }
    }
}
