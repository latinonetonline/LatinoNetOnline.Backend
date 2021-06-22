using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;
using System.Reflection;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Events
{
    static class Extensions
    {
        public static IServiceCollection AddEvents(this IServiceCollection services)
        {
            services.AddSingleton<IEventDispatcher, EventDispatcher>();

            var assemblies = GetAssemblies();
            services.Scan(s => s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }

        public static IEnumerable<Assembly> GetAssemblies()
        {
            var list = new List<string>();
            var stack = new Stack<Assembly>();

            var assembly = Assembly.GetEntryAssembly();

            if (assembly is not null)
            {
                stack.Push(assembly);

                do
                {
                    var asm = stack.Pop();

                    yield return asm;

                    foreach (var reference in asm.GetReferencedAssemblies())
                        if (reference.FullName.Contains("LatinoNetOnline") && !list.Contains(reference.FullName))
                        {
                            stack.Push(Assembly.Load(reference));
                            list.Add(reference.FullName);
                        }

                }
                while (stack.Count > 0);
            }
        }
    }
}