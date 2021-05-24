
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Api;
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Api.JsonConvertes;
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.StartupExtensions;
using LatinoNetOnline.Backend.Shared.Infrastructure.Modules;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LatinoNetOnline.Backend.Bootstrapper")]
namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper
{
    internal static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Assembly assembly)
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

            services.AddSerilog();
            services.AddSwaggerApiVersioning(assembly);

            services.AddModuleRequests();


            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, ILoggerFactory loggerFactory)
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

            app.UseExceptionHandlingOperationResult(loggerFactory.CreateLogger("ExceptionHandler"))
                .UseAllowAnyCors()
                .UseSwaggerApiVersioning(provider)
                .UseAuthenticationOperationResult();

            return app;
        }
    }
}