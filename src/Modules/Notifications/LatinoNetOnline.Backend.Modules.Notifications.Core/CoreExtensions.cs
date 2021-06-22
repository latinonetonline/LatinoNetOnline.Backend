using LatinoNetOnline.Backend.Modules.Notifications.Core.Data;
using LatinoNetOnline.Backend.Modules.Notifications.Core.Options;
using LatinoNetOnline.Backend.Modules.Notifications.Core.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core
{
    static class CoreExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IUserService, UserService>();

            services.AddDbContext<NotificationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                    o => o.MigrationsAssembly("LatinoNetOnline.Backend.Bootstrapper")));

            var valipKeysOptions = configuration.GetSection(nameof(VapidKeysOptions)).Get<VapidKeysOptions>();
            services.AddSingleton(valipKeysOptions);

            return services;
        }
    }
}

