using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Events
{
    record WebinarConfirmedEventInput(Guid Id) : IEvent;
}
