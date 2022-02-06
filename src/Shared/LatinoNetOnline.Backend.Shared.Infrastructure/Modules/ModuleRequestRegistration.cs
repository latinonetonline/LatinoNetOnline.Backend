
using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Modules
{
    internal sealed class ModuleRequestRegistration
    {
        public ModuleRequestRegistration(Type receiverType, Func<IServiceProvider, object, Task<object>> action)
        {
            ReceiverType = receiverType;
            Action = action;
        }

        public Type ReceiverType { get; set; }
        public Func<IServiceProvider, object, Task<object>> Action { get; set; }
    }
}
