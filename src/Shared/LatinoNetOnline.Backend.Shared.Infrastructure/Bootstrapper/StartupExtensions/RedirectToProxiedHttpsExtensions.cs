
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;

using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Rules;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.StartupExtensions
{
    public static class RedirectToProxiedHttpsExtensions
    {
        public static RewriteOptions AddRedirectToProxiedHttps(this RewriteOptions options)
        {
            options.Rules.Add(new RedirectToProxiedHttpsRule());
            return options;
        }

        public static IApplicationBuilder UseRedirectToProxiedHttps(this IApplicationBuilder app)
        {
            RewriteOptions options = new();

            options.AddRedirectToProxiedHttps()
               .AddRedirect("(.*)/$", "$1");  // remove trailing slash
            app.UseRewriter(options);
            return app;
        }
    }
}
