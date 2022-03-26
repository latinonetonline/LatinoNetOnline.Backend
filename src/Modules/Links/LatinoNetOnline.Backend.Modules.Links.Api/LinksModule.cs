using AivenEcommerce.V1.Modules.GitHub.DependencyInjection.Extensions;

using LatinoNetOnline.Backend.Modules.Links.Core.Services;
using LatinoNetOnline.Backend.Shared.Infrastructure.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNetOnline.Backend.Modules.Links.Api
{
    public class LinksModule : Module
    {
        public override void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILinkService, LinkService>();

            services.AddGitHubClient();
        }
    }
}