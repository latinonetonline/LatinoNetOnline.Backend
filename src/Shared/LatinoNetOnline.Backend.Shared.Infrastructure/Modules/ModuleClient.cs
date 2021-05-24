
using LatinoNetOnline.Backend.Shared.Abstractions.Modules;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IBufSerializer = GroBuf.ISerializer;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Modules
{
    internal sealed class ModuleClient : IModuleClient
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IModuleRegistry _moduleRegistry;
        private readonly IBufSerializer _serializer;

        public ModuleClient(IServiceProvider serviceProvider, IModuleRegistry moduleRegistry, IBufSerializer serializer)
        {
            _serviceProvider = serviceProvider;
            _moduleRegistry = moduleRegistry;
            _serializer = serializer;
        }

        public async Task<TResult> GetAsync<TResult>(string path, object moduleRequest) where TResult : class
        {
            var registration = _moduleRegistry.GetRequestRegistration(path);

            if (registration is null)
            {
                throw new InvalidOperationException($"No action has been defined for path: {path}");
            }

            var moduleRequestType = moduleRequest.GetType();

            var action = registration.Action;
            var receiverRequest = TranslateType(moduleRequest, registration.ReceiverType);

            var result = await action(_serviceProvider, receiverRequest);


            return (TResult)TranslateType(result, typeof(TResult));

        }

        public async Task PublishAsync(object moduleBroadcast)
        {
            var tasks = new List<Task>();
            var path = moduleBroadcast.GetType().Name;
            var registrations = _moduleRegistry
                .GetBroadcastRegistration(path)
                .Where(r => r.ReceiverType != moduleBroadcast.GetType());

            foreach (var registration in registrations)
            {
                var action = registration.Action;
                var receiverBroadcast = TranslateType(moduleBroadcast, registration.ReceiverType);
                tasks.Add(action(_serviceProvider, receiverBroadcast));
            }
            await Task.WhenAll(tasks);

        }

        private object TranslateType(object @object, Type type) => _serializer.ChangeType(@object.GetType(), type, @object);


    }
}
