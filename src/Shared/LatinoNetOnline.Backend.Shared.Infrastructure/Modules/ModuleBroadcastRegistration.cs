
using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Modules
{
    sealed class ModuleBroadcastRegistration
    {
        public ModuleBroadcastRegistration(Type receiverType, Func<IServiceProvider, object, Task> action)
        {
            ReceiverType = receiverType;
            Action = action;
        }

        public Type ReceiverType { get; set; }
        public Func<IServiceProvider, object, Task> Action { get; set; }

        public string Key => ReceiverType.Name;
    }
}
