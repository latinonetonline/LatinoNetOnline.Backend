
using LatinoNetOnline.Backend.Modules.Events.Core.Enums;

using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars
{
    record WebinarDto(Guid Id, Guid ProposalId, int Number, long MeetupId, Uri? Streamyard, Uri? LiveStreaming, Uri? Flyer, WebinarStatus Status);
}
