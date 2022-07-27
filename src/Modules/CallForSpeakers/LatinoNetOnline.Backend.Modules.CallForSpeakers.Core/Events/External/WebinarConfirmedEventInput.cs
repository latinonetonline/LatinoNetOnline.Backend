using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Events.External
{
    record WebinarConfirmedEventInput(Guid Id) : IEvent;
}
