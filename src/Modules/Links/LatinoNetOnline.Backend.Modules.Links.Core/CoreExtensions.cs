using AivenEcommerce.V1.Modules.GitHub.DependencyInjection.Extensions;

using LatinoNETOnline.App.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNetOnline.Backend.Modules.Links.Core
{

    public static class CoreExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILinkService, LinkService>();

            services.AddGitHubClient();

            return services;
        }
    }
}