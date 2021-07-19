using LatinoNetOnline.Backend.Shared.Infrastructure.Events;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.DependencyInjection
{
    public static class ModuleExtensions
    {
        public static IServiceCollection RegisterModule<T>(this IServiceCollection services, T module)
            where T : Module
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            module.Load(services, configuration);

            return services;
        }

        public static IServiceCollection RegisterModule<T>(this IServiceCollection services)
            where T : Module
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var constructor = typeof(T).GetConstructors().FirstOrDefault(x => x.IsPublic && !x.IsStatic && !x.ContainsGenericParameters);

            List<object> parameters = new();
            if (constructor is not null)
            {

                foreach (var parameter in constructor.GetParameters())
                {
                    var parameterInstance = services.BuildServiceProvider().GetRequiredService(parameter.ParameterType);

                    parameters.Add(parameterInstance);
                }
            }

            Module? moduleInstance = (Module?)Activator.CreateInstance(typeof(T), parameters.ToArray());
            moduleInstance?.Load(services, configuration);
            return services;
        }

        public static IApplicationBuilder UseRegisterModules(this IApplicationBuilder app)
        {

            List<Type> types = new();
            var assemblies = Extensions.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var typesModule = assembly.GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Module)));

                types.AddRange(typesModule);

            }


            foreach (var module in types)
            {
                var constructor = module.GetConstructors().FirstOrDefault(x => x.IsPublic && !x.IsStatic && !x.ContainsGenericParameters);

                List<object> parameters = new();
                if (constructor is not null)
                {

                    foreach (var parameter in constructor.GetParameters())
                    {
                        var parameterInstance = app.ApplicationServices.GetRequiredService(parameter.ParameterType);

                        parameters.Add(parameterInstance);
                    }
                }

                Module? moduleInstance = (Module?)Activator.CreateInstance(module, parameters.ToArray());
                moduleInstance?.Configure(app);
            }

            return app;

        }

        public static IHost InitRegisterModules(this IHost host)
        {

            List<Type> types = new();
            var assemblies = Extensions.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var typesModule = assembly.GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Module)));

                types.AddRange(typesModule);

            }

            var configuration = host.Services.GetRequiredService<IConfiguration>();

            foreach (var module in types)
            {
                var constructor = module.GetConstructors().FirstOrDefault(x => x.IsPublic && !x.IsStatic && !x.ContainsGenericParameters);

                List<object> parameters = new();
                if (constructor is not null)
                {

                    foreach (var parameter in constructor.GetParameters())
                    {
                        var parameterInstance = host.Services.GetRequiredService(parameter.ParameterType);

                        parameters.Add(parameterInstance);
                    }
                }

                Module? moduleInstance = (Module?)Activator.CreateInstance(module, parameters.ToArray());
                moduleInstance?.InitialConfiguration(configuration);
            }

            return host;

        }
    }
}
