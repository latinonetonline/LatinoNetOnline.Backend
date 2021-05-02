using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using LatinoNetOnline.Backend.Worker.Application.Events;
using LatinoNetOnline.Backend.Worker.Application.Events.Test;

namespace LatinoNetOnline.Backend.Worker.Application
{
    static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddSingleton<IEventHandler<TestEventInput>, TestEventHandler>();



            return services;
        }

        public static IApplicationBuilder UseApplication(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
