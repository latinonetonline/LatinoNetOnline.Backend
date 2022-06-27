
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Middlewares;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.StartupExtensions
{
    public static class AuthenticationOperationResultExtensions
    {
        public static IApplicationBuilder UseAuthenticationOperationResult(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthenticationOperationResultMiddleware>();
            return app;
        }

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddAuthorization(o =>
            {

                o.FallbackPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                       .RequireRole("Admin")
                       .RequireAuthenticatedUser()
                       .Build();

            });


            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.Authority = configuration["Security:Issuer"];
                    bearer.Audience = configuration["Security:Audience"];
                    bearer.RequireHttpsMetadata = true;
                    bearer.SaveToken = true;
                });

            return services;
        }
    }
}
