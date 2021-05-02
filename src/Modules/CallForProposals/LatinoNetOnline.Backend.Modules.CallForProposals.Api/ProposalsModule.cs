using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using LatinoNetOnline.Backend.Modules.CallForProposals.Core;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Data;
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Options;

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LatinoNetOnline.Backend.Bootstrapper")]
namespace LatinoNetOnline.Backend.Modules.CallForProposals.Api
{
    internal static class ProposalsModule
    {
        public static IServiceCollection AddProposalsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCore(configuration);

            return services;
        }

        public static IApplicationBuilder UseProposalsModule(this IApplicationBuilder app)
        {
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