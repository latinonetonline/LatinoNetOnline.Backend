using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using System;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Events.External
{
    record ProposalCreatedEventInput(Guid Id, string Title) : IEvent;
}
