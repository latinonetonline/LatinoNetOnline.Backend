
using Azure.Storage.Blobs;

using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Events.Core.Managers;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Options;
using LatinoNetOnline.Backend.Shared.Infrastructure.DependencyInjection;
using LatinoNetOnline.Backend.Shared.Infrastructure.Modules;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

using Octokit;

using System.IO;


namespace LatinoNetOnline.Backend.Modules.Events.Api
{
    public class EventsModule : Module
    {
        public override void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProposalService, ProposalService>();
            services.AddScoped<IWebinarService, WebinarService>();
            services.AddScoped<IUnavailableDateService, UnavailableDateService>();
            services.AddScoped<IStorageService, BlobStorageService>();
            services.AddScoped<IEmailManager, EmailManager>();

            services.AddScoped<IMeetupService, MeetupService>();
            services.AddScoped<IMetricoolService, MetricoolService>();
            services.AddScoped<IGraphQLManager, GraphQLManager>();
            services.AddScoped<ITokenRefresherManager, TokenRefresherManager>();
            services.AddScoped<IGitHubService, GitHubService>();


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                    o => o.MigrationsAssembly("LatinoNetOnline.Backend.Bootstrapper")));

            services.AddSingleton<IGitHubClient, GitHubClient>(sp =>
            {
                GitHubClient githubClient = new(new ProductHeaderValue(nameof(LatinoNetOnline)));

                Credentials basicAuth = new(configuration["GitHubOptions:Token"]);

                githubClient.Credentials = basicAuth;

                return githubClient;
            });

            services.AddScoped<BlobServiceClient>(sp => new(configuration.GetConnectionString("BlobStorage")));
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

        }

        public override void InitialConfiguration(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            var settingOptions = configuration.GetSection(nameof(SettingOptions)).Get<SettingOptions>();
            SeedData.EnsureSeedData(connectionString, settingOptions);
        }
    }
}