
using Microsoft.AspNetCore.Builder;

using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Middlewares;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.StartupExtensions
{
    public static class AuthenticationOperationResultExtensions
    {
        public static IApplicationBuilder UseAuthenticationOperationResult(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthenticationOperationResultMiddleware>();
            return app;
        }
    }
}
