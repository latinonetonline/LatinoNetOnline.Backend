using LatinoNetOnline.Backend.Modules.Notifications.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LatinoNetOnline.Backend.Modules.Notifications.Api
{
    static class NotificationModule
    {
        public static IServiceCollection AddNotificationModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCore(configuration);

            return services;
        }

        public static IApplicationBuilder UseNotificationModule(this IApplicationBuilder app)
        {
            return app;
        }

        public static IHost InitNotificationModule(this IHost host)
        {
            return host;
        }
    }
}
