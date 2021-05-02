using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Shared.Abstractions.Modules
{
    public interface IModuleBroadcast
    {
        IModuleBroadcast Subscribe<TRequest>(Func<IServiceProvider, TRequest, Task> action)
            where TRequest : class;
    }
}
