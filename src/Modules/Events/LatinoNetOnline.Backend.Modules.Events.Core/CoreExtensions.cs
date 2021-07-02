using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Managers;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNetOnline.Backend.Modules.Events.Core
{
    static class CoreExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IWebinarService, WebinarService>();
            services.AddScoped<IProposalService, ProposalService>();
            services.AddScoped<IMeetupService, MeetupService>();
            services.AddScoped<IGraphQLManager, GraphQLManager>();
            services.AddScoped<ITokenRefresherManager, TokenRefresherManager>();

            services.AddDbContext<EventDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                    o => o.MigrationsAssembly("LatinoNetOnline.Backend.Bootstrapper")));

            return services;
        }
    }
}
