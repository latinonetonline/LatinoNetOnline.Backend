using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Events.External;
using LatinoNetOnline.Backend.Modules.Events.Core.Managers;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;
using LatinoNetOnline.Backend.Shared.Abstractions.Options;
using LatinoNetOnline.Backend.Shared.Infrastructure.DependencyInjection;
using LatinoNetOnline.Backend.Shared.Infrastructure.Modules;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNetOnline.Backend.Modules.Events.Api
{
    public class EventsModule : Module
    {
        public override void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IWebinarService, WebinarService>();
            services.AddScoped<IProposalService, ProposalService>();
            services.AddScoped<IMeetupService, MeetupService>();
            services.AddScoped<IMetricoolService, MetricoolService>();
            services.AddScoped<IGraphQLManager, GraphQLManager>();
            services.AddScoped<ITokenRefresherManager, TokenRefresherManager>();

            services.AddDbContext<EventDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                    o => o.MigrationsAssembly("LatinoNetOnline.Backend.Bootstrapper")));
        }

        public override void Configure(IApplicationBuilder app)
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
                        .HandleAsync(input))

                .Subscribe<ProposalDeletedEventInput>((sp, input)
                    => sp.CreateScope().ServiceProvider
                        .GetService<IEventHandler<ProposalDeletedEventInput>>()
                        .HandleAsync(input));
        }


        public override void InitialConfiguration(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            var settingOptions = configuration.GetSection(nameof(SettingOptions)).Get<SettingOptions>();
            SeedData.EnsureSeedData(connectionString, settingOptions);
        }
    }
}
