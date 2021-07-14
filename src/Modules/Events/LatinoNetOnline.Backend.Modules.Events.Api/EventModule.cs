using LatinoNetOnline.Backend.Modules.Events.Core;
using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Events;
using LatinoNetOnline.Backend.Modules.Events.Core.Events.External;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;
using LatinoNetOnline.Backend.Shared.Abstractions.Options;
using LatinoNetOnline.Backend.Shared.Infrastructure.Modules;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LatinoNetOnline.Backend.Modules.Events.Api
{
    static class EventModule
    {
        public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCore(configuration);

            return services;
        }

        public static IApplicationBuilder UseEventsModule(this IApplicationBuilder app)
        {
            app.UseModuleRequests()
                .Subscribe<GetWebinarInput>("modules/webinars/get", async (sp, query) =>
                {
                    var handler = sp.GetRequiredService<IWebinarService>();
                    return await handler.GetByIdAsync(query);
                });


            app.UseModuleBroadcast()
                .Subscribe<ProposalCreatedEventInput>((sp, input)
                    => sp.CreateScope().ServiceProvider
                        .GetService<IEventHandler<ProposalCreatedEventInput>>()
                        .HandleAsync(input))

                .Subscribe<ProposalUpdatedEventInput>((sp, input)
                    => sp.CreateScope().ServiceProvider
                        .GetService<IEventHandler<ProposalUpdatedEventInput>>()
                        .HandleAsync(input));

            return app;
        }

        public static IHost InitEventsModule(this IHost host)
        {
            var configuration = host.Services.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("Default");
            var settingOptions = configuration.GetSection(nameof(SettingOptions)).Get<SettingOptions>();
            SeedData.EnsureSeedData(connectionString, settingOptions);

            return host;
        }
    }
}
