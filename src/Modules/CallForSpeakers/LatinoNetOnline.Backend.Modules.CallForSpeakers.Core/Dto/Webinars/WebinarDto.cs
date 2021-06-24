using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Webinars
{
    record WebinarDto(Guid Id, Guid ProposalId, long MeetupId, Uri? LiveStreaming,  Uri? Flyer);
}
