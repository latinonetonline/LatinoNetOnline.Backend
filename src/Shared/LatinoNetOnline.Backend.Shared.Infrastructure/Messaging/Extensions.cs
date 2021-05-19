
using LatinoNetOnline.Backend.Shared.Abstractions.Messaging;

using Microsoft.Extensions.DependencyInjection;

using ModularMonolith.Shared.Infrastructure;
using ModularMonolith.Shared.Infrastructure.Messaging;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Messaging
{
    internal static class Extensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBroker, InMemoryMessageBroker>();
            services.AddSingleton<IMessageChannel, MessageChannel>();
            services.AddSingleton<IAsyncMessageDispatcher, AsyncMessageDispatcher>();
            var options = services.GetOptions<MessagingOptions>("Messaging");
            services.AddSingleton(options);

            if (options.UseBackgroundDispatcher)
            {
                services.AddHostedService<BackgroundDispatcher>();
            }

            return services;
        }
    }
}