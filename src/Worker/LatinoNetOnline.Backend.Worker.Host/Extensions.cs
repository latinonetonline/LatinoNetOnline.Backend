
using LatinoNetOnline.Backend.Shared.Abstractions.Modules;
using LatinoNetOnline.Backend.Worker.Application;
using LatinoNetOnline.Backend.Worker.Application.Events;
using LatinoNetOnline.Backend.Worker.Application.Events.Test;
using LatinoNetOnline.Backend.Worker.Host.BackgroundTasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Worker.Host
{
    static class Extensions
    {
        public static IServiceCollection AddWorker(this IServiceCollection services)
        {
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHostedService<QueuedHostedService>();
            services.AddApplication();

            return services;
        }

        public static IApplicationBuilder UseWorker(this IApplicationBuilder app)
        {
            app.UseApplication();

            app.UseModuleBroadcast()
                .Subscribe<TestEventInput>((sp, query) =>
                {
                    var backgroundTaskQueue = sp.GetRequiredService<IBackgroundTaskQueue>();

                    backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
                    {
                        var handler = sp.GetRequiredService<IEventHandler<TestEventInput>>();
                        await handler.Handle(query, token);
                    });

                    return Task.CompletedTask;
                });

            return app;
        }
    }
}
