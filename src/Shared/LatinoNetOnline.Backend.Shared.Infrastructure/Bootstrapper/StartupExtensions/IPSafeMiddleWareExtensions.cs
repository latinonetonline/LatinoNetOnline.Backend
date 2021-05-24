
using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Middlewares;

using Microsoft.AspNetCore.Builder;

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
