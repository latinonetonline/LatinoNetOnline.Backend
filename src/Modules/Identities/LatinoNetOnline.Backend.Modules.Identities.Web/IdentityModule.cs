
using IdentityModel;

using IdentityServer4;

using IdentityServerHost.Models;

using LatinoNetOnline.Backend.Modules.Identities.Web.Controllers;
using LatinoNetOnline.Backend.Modules.Identities.Web.Data;
using LatinoNetOnline.Backend.Modules.Identities.Web.Dto.Users;
using LatinoNetOnline.Backend.Modules.Identities.Web.Options;
using LatinoNetOnline.Backend.Modules.Identities.Web.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Options;
using LatinoNetOnline.Backend.Shared.Infrastructure.DependencyInjection;
using LatinoNetOnline.Backend.Shared.Infrastructure.Modules;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace LatinoNetOnline.Backend.Modules.Identities.Web
{
    public class IdentityModule : Module
    {
        private readonly IWebHostEnvironment _env;

        public IdentityModule(IWebHostEnvironment env)
        {
            _env = env;
        }

        public override void Load(IServiceCollection services, IConfiguration configuration)
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

                options.EmitStaticAudienceClaim = true;

                options.UserInteraction.LoginUrl = $"/{BaseMvcController.BasePath}/Account/Login";
                options.UserInteraction.ErrorUrl = $"/{BaseMvcController.BasePath}/Home/Error";
                options.UserInteraction.ConsentUrl = $"/{BaseMvcController.BasePath}/Consent";
                options.UserInteraction.DeviceVerificationUrl = $"/{BaseMvcController.BasePath}/Device";
                options.UserInteraction.LogoutUrl = $"/{BaseMvcController.BasePath}/Account/Logout";

            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.GetClients(configuration))
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<IdentityProfileService>();

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
                    //options.scope.add("https://www.googleapis.com/auth/calendar");
                    //options.SaveTokens = true;
                    //options.AccessType = "offline";
                    //options.Events.OnCreatingTicket = (ticket) =>
                    //{
                    //    return Task.CompletedTask;
                    //};
                });

            services.AddLocalApiAuthentication(principal =>
            {
                string role = principal.FindFirstValue(JwtClaimTypes.Role);
                principal.Identities.First().AddClaim(new Claim(JwtClaimTypes.Role, role));

                return Task.FromResult(principal);
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy =>
                {
                    policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
                    policy.RequireRole("Admin");
                    policy.RequireAuthenticatedUser();
                    // custom requirements
                });
            });

            services.AddScoped<IUserService, UserService>();
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(new FileInfo(typeof(ApplicationDbContext).Assembly.Location).DirectoryName ?? string.Empty, "IdentitySite")),
                RequestPath = new PathString($"/assets/{BaseMvcController.BasePath}")
            });
            app.UseIdentityServer();

            app.UseModuleRequests()
                .Subscribe<GetAllUserInput>("modules/users/get", async (sp, query) =>
                {
                    var handler = sp.GetRequiredService<IUserService>();
                    return await handler.GetAllAsync(query);
                });

            app.UseAuthorization();
        }

        public override void InitialConfiguration(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            var settingOptions = configuration.GetSection(nameof(SettingOptions)).Get<SettingOptions>();
            var identityOptions = configuration.GetSection(nameof(Options.IdentityOptions)).Get<Options.IdentityOptions>();
            SeedData.EnsureSeedData(connectionString, settingOptions, identityOptions);
        }
    }
}
