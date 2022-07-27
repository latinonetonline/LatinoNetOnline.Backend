
using LatinoNetOnline.Backend.Modules.Webinars.Api;
using LatinoNetOnline.Backend.Modules.Links.Api;
using LatinoNetOnline.Backend.Modules.Notifications.Api;
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper;
using LatinoNetOnline.Backend.Shared.Infrastructure.DependencyInjection;

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
            services.AddInfrastructure(Configuration, this.GetType().Assembly);
            services.RegisterModule<WebinarsModule>();
            services.RegisterModule<LinksModule>();
            services.RegisterModule<NotificationModule>();
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider, ILoggerFactory loggerFactory)
        {
            app.UseInfrastructure(Environment, provider, loggerFactory, Configuration);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRegisterModules();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", context => context.Response.WriteAsync("Modular Monolith API"));
            });
        }
    }
}
