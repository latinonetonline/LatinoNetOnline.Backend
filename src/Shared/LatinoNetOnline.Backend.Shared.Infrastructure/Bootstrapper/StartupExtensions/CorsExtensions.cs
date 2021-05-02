
using Microsoft.AspNetCore.Builder;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.StartupExtensions
{
    public static class CorsExtensions
    {
        public static IApplicationBuilder UseAllowAnyCors(this IApplicationBuilder app)
        {
            return app.UseCors(builder => builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod().AllowAnyHeader());
        }
    }
}
