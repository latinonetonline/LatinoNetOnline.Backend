
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Api;
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Api.JsonConvertes;
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.StartupExtensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ModularMonolith.Shared.Infrastructure;

using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LatinoNetOnline.Backend.Bootstrapper")]
namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper
{
    internal static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
        {
            services.AddControllersWithViews()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                })
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
                });
            services.AddInfrastructure();
            services.AddJwtAuthentication(configuration);
            services.AddSerilog();
            services.AddSwaggerApiVersioning(assembly);
            services.AddForwardedHeaders();
            services.AddHttpContextAccessor();
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            app.UseForwardedHeaders()

            .UseRedirectToProxiedHttps();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts()

                .UseIPSafe();
            }

            app.UseInfrastructure();

            app.UseExceptionHandlingOperationResult(loggerFactory.CreateLogger("ExceptionHandler"))
                .UseAllowAnyCors()
                .UseSwaggerApiVersioning(provider, configuration)
                .UseAuthenticationOperationResult();

            return app;
        }
    }
}