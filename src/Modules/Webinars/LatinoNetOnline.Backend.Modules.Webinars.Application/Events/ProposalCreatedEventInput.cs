using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Events
{
    record ProposalCreatedEventInput(Guid Id, string Title) : IEvent;
}
