
using GroBuf;
using GroBuf.DataMembersExtracters;

using LatinoNetOnline.Backend.Shared.Abstractions.Modules;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Modules
{
    public static class Extensions
    {
        public static IServiceCollection AddModuleRequests(this IServiceCollection services)
        {
            services.AddModuleRegistry();
            services.AddSingleton<IModuleSubscriber, ModuleSubscriber>();
            services.AddSingleton<IModuleBroadcast, ModuleBroadcast>();
            services.AddTransient<IModuleClient, ModuleClient>();

            return services;
        }

        public static IModuleSubscriber UseModuleRequests(this IApplicationBuilder app)
            => app.ApplicationServices.GetRequiredService<IModuleSubscriber>();

        public static IModuleBroadcast UseModuleBroadcast(this IApplicationBuilder app)
            => app.ApplicationServices.GetRequiredService<IModuleBroadcast>();

        private static void AddModuleRegistry(this IServiceCollection services)
        {
            services.AddSingleton<IModuleRegistry, ModuleRegistry>();
            services.AddSingleton<ISerializer>(new Serializer(new PropertiesExtractor(), options: GroBufOptions.WriteEmptyObjects));
        }
    }
}
