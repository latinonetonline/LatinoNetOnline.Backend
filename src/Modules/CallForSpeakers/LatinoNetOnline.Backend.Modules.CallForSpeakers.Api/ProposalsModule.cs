using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Events.External;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;
using LatinoNetOnline.Backend.Shared.Abstractions.Options;
using LatinoNetOnline.Backend.Shared.Infrastructure.Modules;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

using System.IO;


namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Api
{
    static class ProposalsModule
    {
        public static IServiceCollection AddCallForSpeakersModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCore(configuration);

            return services;
        }

        public static IApplicationBuilder UseCallForSpeakersModule(this IApplicationBuilder app)
        {

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(new FileInfo(typeof(ApplicationDbContext).Assembly.Location).DirectoryName ?? string.Empty, "Files")),
                RequestPath = new PathString("/callforspeakers-module")
            });

            app.UseModuleRequests()
                .Subscribe<GetProposalInput>("modules/proposals/get", async (sp, query) =>
                {
                    var handler = sp.GetRequiredService<IProposalService>();
                    return await handler.GetByIdAsync(query);
                });

            app.UseModuleBroadcast()
                .Subscribe<WebinarConfirmedEventInput>((sp, input)
                    => sp.CreateScope().ServiceProvider
                        .GetService<IEventHandler<WebinarConfirmedEventInput>>()
                        .HandleAsync(input));

            return app;
        }

        public static IHost InitCallForSpeakersModule(this IHost host)
        {
            var configuration = host.Services.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("Default");
            var settingOptions = configuration.GetSection(nameof(SettingOptions)).Get<SettingOptions>();
            SeedData.EnsureSeedData(connectionString, settingOptions);

            return host;
        }
    }
}