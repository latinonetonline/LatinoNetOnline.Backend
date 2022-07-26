using AivenEcommerce.V1.Modules.GitHub.DependencyInjection.Extensions;

using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Events.External;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Managers;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;
using LatinoNetOnline.Backend.Shared.Abstractions.Options;
using LatinoNetOnline.Backend.Shared.Infrastructure.DependencyInjection;
using LatinoNetOnline.Backend.Shared.Infrastructure.Modules;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

using System.IO;


namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Api
{
    public class CallForSpeakersModule : Module
    {
        public override void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISpeakerService, SpeakerService>();
            services.AddScoped<IProposalService, ProposalService>();
            services.AddScoped<IWebinarService, WebinarService>();
            services.AddScoped<IUnavailableDateService, UnavailableDateService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IMetricoolService, MetricoolService>();
            services.AddScoped<IWebsiteService, WebsiteService>();
            services.AddScoped<IEmailManager, EmailManager>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                    o => o.MigrationsAssembly("LatinoNetOnline.Backend.Bootstrapper")));

            services.AddGitHubClient();
        }

        public override void Configure(IApplicationBuilder app)
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
        }

        public override void InitialConfiguration(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            var settingOptions = configuration.GetSection(nameof(SettingOptions)).Get<SettingOptions>();
            SeedData.EnsureSeedData(connectionString, settingOptions);
        }
    }
}