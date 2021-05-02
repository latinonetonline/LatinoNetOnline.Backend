using LatinoNetOnline.Backend.Worker.Host.Events;

namespace LatinoNetOnline.Backend.Worker.Application.Events.Test
{
    record TestEventInput(string Message) : IEventInput;

}
