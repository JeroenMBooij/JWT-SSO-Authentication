using AuthenticationServer.Web.Middleware.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationServer.Web.Middleware
{
    public static class EndpointService
    {
        public static IServiceCollection AddMyEndpoints(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .Build());
            });

            services.AddMvc(options => {
                options.Filters.Add<ValidationFilter>(1);
                options.Filters.Add<ErrorFilter>();
                options.Filters.Add(new ConsumesAttribute("application/json"));
            })
            .AddFluentValidation(mvcConfig => mvcConfig.RegisterValidatorsFromAssemblyContaining<Startup>())
            .AddNewtonsoftJson();

            //https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.1#automatic-http-400-responses 
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });

            #region Controller Attributes
            services.Scan(scan => scan
                .FromAssemblyOf<IWebAssembly>()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Attribute")))
                .AsSelf()
                .WithScopedLifetime()
            );
            #endregion


            return services;
        }
    }
}
