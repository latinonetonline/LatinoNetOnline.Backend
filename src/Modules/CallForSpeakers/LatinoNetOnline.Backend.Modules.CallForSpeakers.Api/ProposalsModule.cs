using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Shared.Abstractions.Options;

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