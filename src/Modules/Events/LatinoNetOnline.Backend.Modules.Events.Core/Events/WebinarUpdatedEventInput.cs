using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Events
{
    record WebinarUpdatedEventInput(Guid Id) : IEvent;

}
