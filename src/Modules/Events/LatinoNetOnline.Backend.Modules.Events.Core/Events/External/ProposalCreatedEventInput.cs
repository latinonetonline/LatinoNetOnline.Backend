using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Events.External
{
    public record ProposalCreatedEventInput(Guid Id, string Title) : IEvent;
}
