using AivenEcommerce.V1.Modules.GitHub.DependencyInjection.Extensions;

using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Managers;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core
{

    public static class CoreExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISpeakerService, SpeakerService>();
            services.AddScoped<IProposalService, ProposalService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IEmailManager, EmailManager>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                    o => o.MigrationsAssembly("LatinoNetOnline.Backend.Bootstrapper")));

            services.AddGitHubClient();

            return services;
        }
    }
}