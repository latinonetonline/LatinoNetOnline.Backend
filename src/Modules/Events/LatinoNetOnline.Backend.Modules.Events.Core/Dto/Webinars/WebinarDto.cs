
using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars
{
    record WebinarDto(Guid Id, Guid ProposalId,string Title, string Description, long MeetupId, DateTime StartDateTime, Uri? LiveStreaming, Uri? Flyer);
}
