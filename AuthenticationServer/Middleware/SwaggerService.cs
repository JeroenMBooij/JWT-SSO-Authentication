using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AuthenticationServer.Web.Middleware
{
    public static class SwaggerService
    {
        public static IServiceCollection AddMySwagger(this IServiceCollection services)
        {
            #region Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Jiren's Authentication Server",
                    Description = @"An API to handle all your application authentication and authorization needs.
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
            });
            #endregion

            return services;
        }
    }
}
