using LatinoNetOnline.Backend.Modules.Notifications.Core;
using LatinoNetOnline.Backend.Modules.Notifications.Core.Events.External;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;
using LatinoNetOnline.Backend.Shared.Infrastructure.Modules;

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
            app.UseModuleBroadcast().Subscribe<ProposalCreatedEventInput>((sp, input)
                => sp.CreateScope().ServiceProvider
                        .GetService<IEventHandler<ProposalCreatedEventInput>>()
                        .HandleAsync(input));


            return app;
        }

        public static IHost InitNotificationModule(this IHost host)
        {
            return host;
        }
    }
}
