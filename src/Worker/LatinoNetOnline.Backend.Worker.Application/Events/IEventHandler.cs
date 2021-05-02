using LatinoNetOnline.Backend.Worker.Host.Events;

using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Worker.Application.Events
{
    interface IEventHandler<TEventInput> where TEventInput : IEventInput
    {
        Task Handle(TEventInput input, CancellationToken cancellationToken);
    }
}
