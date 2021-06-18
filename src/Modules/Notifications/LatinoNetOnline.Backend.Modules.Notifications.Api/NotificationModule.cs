using LatinoNetOnline.Backend.Modules.Notifications.Api.Options;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WebPushDemo.Models;

namespace LatinoNetOnline.Backend.Modules.Notifications.Api
{
    static class NotificationModule
    {
        public static IServiceCollection AddNotificationModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NotificationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                    o => o.MigrationsAssembly("LatinoNetOnline.Backend.Bootstrapper")));

            var valipKeysOptions = configuration.GetSection(nameof(VapidKeysOptions)).Get<VapidKeysOptions>();
            services.AddSingleton(valipKeysOptions);

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
