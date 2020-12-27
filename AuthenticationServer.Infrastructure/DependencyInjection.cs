using AuthenticationServer.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace AuthenticationServer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {

            #region Auto Mapper
            services.AddAutoMapper(config =>
            {
                config.RecognizeDestinationPrefixes(new[] { "Tenant", "User", "Role", "Domain", "Language", "Dashboard" });
                config.RecognizePrefixes(new[] { "Tenant", "User", "Role", "Domain", "Language", "Dashboard" });

            }, typeof(DependencyInjection));
            #endregion

            return services;
        }

        public static IHostBuilder CreateInfrastructureBuilder(this IHostBuilder builder)
        {
            #region Serial Logger
            builder.UseSerilog((context, configuration) =>
            {
                configuration.Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.Console()
                    .WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(context.Configuration["BaseUrls:ElasticServer"]))
                        {
                            IndexFormat = $"{context.Configuration["ApplicationName"].ToLower().Replace(" ", "-")}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.Now:yyyy-MM}",
                            AutoRegisterTemplate = true,
                            NumberOfShards = 2,
                            NumberOfReplicas = 1
                        })
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .ReadFrom.Configuration(context.Configuration);
            });
            #endregion


            return builder;
        }
    }
}
