using LatinoNetOnline.Backend.Modules.Links.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace LatinoNetOnline.Backend.Modules.Links.Api
{
    static class LinksModule
    {
        public static IServiceCollection AddLinksModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCore(configuration);

            return services;
        }

        public static IApplicationBuilder UseLinksModule(this IApplicationBuilder app)
        {
            return app;
        }

        public static IHost InitLinksModule(this IHost host)
        {
            return host;
        }
    }
}