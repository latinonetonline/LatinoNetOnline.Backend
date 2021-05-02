
using Duende.IdentityServer;

using IdentityServerHost.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

using LatinoNetOnline.Backend.Modules.Identities.Web.Data;
using LatinoNetOnline.Backend.Modules.Identities.Web.Dto;
using LatinoNetOnline.Backend.Modules.Identities.Web.Options;
using LatinoNetOnline.Backend.Modules.Identities.Web.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Modules;
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Options;

using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("LatinoNetOnline.Backend.Bootstrapper")]
namespace LatinoNetOnline.Backend.Modules.Identities.Web
{
    static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                    o => o.MigrationsAssembly(typeof(Config).Assembly.FullName)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;

                options.UserInteraction.LoginUrl = "/identities-module/Account/Login";
            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.GetClients(configuration))
                .AddAspNetIdentity<ApplicationUser>();

            builder.AddDeveloperSigningCredential();

            services.Configure<Microsoft.AspNetCore.Identity.IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });

            var identityOptions = configuration.GetSection(nameof(Options.IdentityOptions)).Get<Options.IdentityOptions>();
            services.AddSingleton(identityOptions);



            var googleOptions = configuration.GetSection(nameof(GoogleOptions)).Get<GoogleOptions>();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to https://localhost:5001/signin-google
                    options.CallbackPath = new PathString("/callbacks/google");
                    options.ClientId = googleOptions.ClientId;
                    options.ClientSecret = googleOptions.ClientSecret;
                });

            services.AddLocalApiAuthentication(principal =>
            {
                //string role = principal.FindFirstValue(ClaimTypes.Role);
                //principal.Identities.First().AddClaim(new Claim("role", role));

                return Task.FromResult(principal);
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy =>
                {
                    policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    // custom requirements
                });
            });

            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IApplicationBuilder UseIdentityModule(this IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(new FileInfo(typeof(ApplicationDbContext).Assembly.Location).DirectoryName ?? string.Empty, "IdentitySite")),
                RequestPath = new PathString("/identity-module")
            });
            app.UseIdentityServer();

            app.UseModuleRequests()
                .Subscribe<GetUserInput>("modules/users/get", async (sp, query) =>
                {
                    var handler = sp.GetRequiredService<IUserService>();
                    return await handler.GetByIdAsync(query.Id.ToString());
                });

            app.UseAuthorization();
            return app;
        }

        public static IHost InitIdentityModule(this IHost host)
        {
            var configuration = host.Services.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("Default");
            var settingOptions = configuration.GetSection(nameof(SettingOptions)).Get<SettingOptions>();
            SeedData.EnsureSeedData(connectionString, settingOptions);

            return host;
        }
    }
}
