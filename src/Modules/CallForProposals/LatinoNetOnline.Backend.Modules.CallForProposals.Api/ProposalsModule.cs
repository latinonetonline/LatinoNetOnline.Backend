using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using LatinoNetOnline.Backend.Modules.CallForProposals.Core;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Data;

using System.Runtime.CompilerServices;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.IO;
using LatinoNetOnline.Backend.Shared.Abstractions.Options;

[assembly: InternalsVisibleTo("LatinoNetOnline.Backend.Bootstrapper")]
namespace LatinoNetOnline.Backend.Modules.CallForProposals.Api
{
    static class ProposalsModule
    {
        public static IServiceCollection AddProposalsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCore(configuration);

            return services;
        }

        public static IApplicationBuilder UseProposalsModule(this IApplicationBuilder app)
        {

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(new FileInfo(typeof(ApplicationDbContext).Assembly.Location).DirectoryName ?? string.Empty, "Files")),
                RequestPath = new PathString("/proposals-module")
            });

            return app;
        }

        public static IHost InitProposalsModule(this IHost host)
        {
            var configuration = host.Services.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("Default");
            var settingOptions = configuration.GetSection(nameof(SettingOptions)).Get<SettingOptions>();
            SeedData.EnsureSeedData(connectionString, settingOptions);

            return host;
        }
    }
}