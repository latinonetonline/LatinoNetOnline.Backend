
using LatinoNetOnline.Backend.Modules.Events.Core.Enums;

using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars
{
    record WebinarDto(Guid Id, Guid ProposalId, string Title, string Description, int Number, long MeetupId, DateTime StartDateTime, Uri? Streamyard, Uri? LiveStreaming, Uri? Flyer, WebinarStatus Status);
}
