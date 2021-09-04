using AuthenticationServer.Web.Middleware.Filters.Swagger;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AuthenticationServer.Web.Middleware
{
    public static class SwaggerService
    {
        public static IServiceCollection AddMySwagger(this IServiceCollection services, IConfiguration config)
        {
            #region Swagger
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, SwaggerProducesFilter>());

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Jeroen's Authentication Server",
                    Description = $@"<a href=""{config["BaseUrls:Dashboard"]}""><h1>Go To Configuration Cockpit</h1></a>
                                    <br/>
                                    An API to handle all your application authentication and authorization needs.
                                    By integrating this service you can configure and monitor everything your users do on your applications. 
                                    To start using this service, become a Tenant now!",
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
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                options.ExampleFilters();
                options.SchemaFilter<SwaggerSchemaFilter>();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                /*
                #region Authentication Schema
                var securityScheme = new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "tenant-authorization",
                    Description = @"Use your tenant Jwt to authenticate every call you make to the the authentication server.",
                    Scheme = "oauth1"
                };
                options.AddSecurityDefinition("Jwt Token", securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            securityScheme,
                            new List<string>()
                        }
                    });
                #endregion*/

            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
            #endregion

            return services;
        }
    }
}
