using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Shared.Abstractions.Modules
{
    internal sealed class ModuleBroadcast : IModuleBroadcast
    {
        private readonly IModuleRegistry _registry;

        public ModuleBroadcast(IModuleRegistry registry)
            => _registry = registry;

        public IModuleBroadcast Subscribe<TRequest>(Func<IServiceProvider, TRequest, Task> action) where TRequest : class
        {
            _registry.AddBroadcastAction(typeof(TRequest), (sp, o) => action(sp, (TRequest)o));
            return this;
        }
    }
}
