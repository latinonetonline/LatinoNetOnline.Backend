using AivenEcommerce.V1.Modules.GitHub.DependencyInjection.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Services;

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LatinoNetOnline.Backend.Modules.CallForProposals.Api")]
namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core
{

    public static class CoreExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISpeakerService, SpeakerService>();
            services.AddScoped<IProposalService, ProposalService>();
            services.AddScoped<IStorageService, StorageService>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                    o => o.MigrationsAssembly("LatinoNetOnline.Backend.Bootstrapper")));

            services.AddGitHubClient();

            return services;
        }
    }
}