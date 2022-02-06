
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Modules
{
    internal class ModuleRegistry : IModuleRegistry
    {
        private readonly IDictionary<string, ModuleRequestRegistration> _requestActions;
        private readonly IList<ModuleBroadcastRegistration> _broadcastActions;

        public ModuleRegistry()
        {
            _broadcastActions = new List<ModuleBroadcastRegistration>();
            _requestActions = new Dictionary<string, ModuleRequestRegistration>();
        }

        public ModuleRequestRegistration GetRequestRegistration(string path)
            => _requestActions[path];

        public IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistration(string path)
            => _broadcastActions.Where(r => r.Key == path);

        public bool TryAddRequestAction(string path, Type receiverType, Func<IServiceProvider, object, Task<object>> action)
        {
            var registration = new ModuleRequestRegistration(receiverType, action);

            var isValid = _requestActions.TryAdd(path, registration);

            return isValid;
        }

        public void AddBroadcastAction(Type receiverType, Func<IServiceProvider, object, Task> action)
        {
            if (string.IsNullOrWhiteSpace(receiverType.Namespace))
            {
                throw new InvalidOperationException("Missing namespace.");
            }

            var registration = new ModuleBroadcastRegistration(receiverType, action);

            _broadcastActions.Add(registration);
        }
    }
}
