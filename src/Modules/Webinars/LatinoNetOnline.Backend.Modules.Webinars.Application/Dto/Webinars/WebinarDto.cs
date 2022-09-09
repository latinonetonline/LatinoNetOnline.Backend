using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Webinars
{
    record WebinarDto(Guid Id, Guid ProposalId, int Number, long MeetupId, DateTime StartDateTime, Uri? Streamyard, Uri? LiveStreaming, Uri? Flyer, int Status);
}
