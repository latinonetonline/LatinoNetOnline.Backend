
using GroBuf;
using GroBuf.DataMembersExtracters;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNetOnline.Backend.Shared.Abstractions.Modules
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

            //var eventTypes = AppDomain.CurrentDomain
            //    .GetAssemblies()
            //    .Where(a => a.FullName.Contains("Cine"))
            //    .SelectMany(a => a.GetTypes())
            //    .Where(t => t.IsClass && typeof(IEvent).IsAssignableFrom(t))
            //    .ToList();

            //builder.Services.AddSingleton<IModuleRegistry>(provider =>
            //{
            //    var logger = provider.GetService<ILogger<IModuleRegistry>>();
            //    var registry = new ModuleRegistry(logger);

            //    foreach (var type in eventTypes)
            //    {
            //        registry.AddBroadcastAction(type, (sp, @event) =>
            //        {
            //            var dispatcher = sp.GetService<IEventDispatcher>();
            //            return (Task)dispatcher.GetType()
            //                .GetMethod(nameof(dispatcher.PublishAsync))
            //                .MakeGenericMethod(type)
            //                .Invoke(dispatcher, new[] { @event });
            //        });
            //    }

            //    return registry;
            //});
        }
    }
}
