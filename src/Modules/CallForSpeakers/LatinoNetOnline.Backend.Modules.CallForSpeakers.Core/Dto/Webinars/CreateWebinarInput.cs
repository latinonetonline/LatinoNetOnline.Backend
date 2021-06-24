using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Webinars
{
    record CreateWebinarInput(Guid ProposalId, long MeetupId);
}