using LatinoNetOnline.Backend.Modules.Notifications.Core.Data;
using LatinoNetOnline.Backend.Modules.Notifications.Core.Events.External;
using LatinoNetOnline.Backend.Modules.Notifications.Core.Options;
using LatinoNetOnline.Backend.Modules.Notifications.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;
using LatinoNetOnline.Backend.Shared.Infrastructure.DependencyInjection;
using LatinoNetOnline.Backend.Shared.Infrastructure.Modules;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNetOnline.Backend.Modules.Notifications.Api
{
    public class NotificationModule : Module
    {
        public override void Load(IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IUserService, UserService>();

            services.AddDbContext<NotificationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                    o => o.MigrationsAssembly("LatinoNetOnline.Backend.Bootstrapper")));

            var valipKeysOptions = configuration.GetSection(nameof(VapidKeysOptions)).Get<VapidKeysOptions>();
            services.AddSingleton(valipKeysOptions);
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseModuleBroadcast().Subscribe<ProposalCreatedEventInput>((sp, input)
                => sp.CreateScope().ServiceProvider
                        .GetService<IEventHandler<ProposalCreatedEventInput>>()
                        .HandleAsync(input));
        }
    }
}
