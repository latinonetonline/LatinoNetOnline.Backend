
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.StartupExtensions
{
    public static class OptionExtensions
    {
        public static IServiceCollection AddOptions<T, K>(this IServiceCollection services, IConfiguration configuration) where K : class, T where T : class
        {
            services.Configure<K>(
                configuration.GetSection(typeof(K).Name));

            services.AddSingleton<T>(sp =>
               sp.GetRequiredService<IOptions<K>>().Value);

            return services;
        }
    }
}
