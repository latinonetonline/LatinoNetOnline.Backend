using Duende.IdentityServer;

using LatinoNetOnline.Backend.Modules.CallForProposals.Api;
using LatinoNetOnline.Backend.Modules.Links.Api;
using LatinoNetOnline.Backend.Modules.Identities.Web;
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LatinoNetOnline.Backend.Bootstrapper
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; set; }
        public IWebHostEnvironment Environment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(this.GetType().Assembly);
            services.AddProposalsModule(Configuration);
            services.AddLinksModule(Configuration);
            services.AddIdentityModule(Configuration, Environment);
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider, ILoggerFactory loggerFactory)
        {
            app.UseInfrastructure(Environment, provider, loggerFactory);
            app.UseRouting();
            app.UseProposalsModule();
            app.UseLinksModule();
            app.UseIdentityModule();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization(IdentityServerConstants.LocalApi.PolicyName);
                endpoints.MapGet("/", context => context.Response.WriteAsync("Modular Monolith API"));
            });
        }
    }
}
