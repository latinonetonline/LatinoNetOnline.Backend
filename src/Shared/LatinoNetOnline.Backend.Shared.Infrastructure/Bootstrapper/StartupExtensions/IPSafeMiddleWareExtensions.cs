
using Microsoft.AspNetCore.Builder;

using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Middlewares;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.StartupExtensions
{
    public static class IPSafeMiddleWareExtensions
    {
        public static IApplicationBuilder UseIPSafe(this IApplicationBuilder app)
        {
            app.UseMiddleware<IPSafeMiddleWare>();
            return app;
        }
    }
}
