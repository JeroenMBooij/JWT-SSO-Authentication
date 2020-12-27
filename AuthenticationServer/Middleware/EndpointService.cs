using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationServer.Web.Middleware
{
    public static class EndpointService
    {
        public static IServiceCollection AddMyEndpoints(this IServiceCollection services)
        {
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}).AddCookie();

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

            return services;
        }
    }
}
