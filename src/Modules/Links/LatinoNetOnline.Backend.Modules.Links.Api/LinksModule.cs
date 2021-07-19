using AivenEcommerce.V1.Modules.GitHub.DependencyInjection.Extensions;

using LatinoNetOnline.Backend.Modules.Links.Core;
using LatinoNetOnline.Backend.Shared.Infrastructure.DependencyInjection;

using LatinoNETOnline.App.Infrastructure.Services;

using Microsoft.AspNetCore.Builder;
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