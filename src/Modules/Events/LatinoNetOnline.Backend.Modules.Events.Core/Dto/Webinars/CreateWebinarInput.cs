
using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars
{
    record CreateWebinarInput(Guid ProposalId, string Title, string Description, long MeetupId, DateTime StartDateTime);
}