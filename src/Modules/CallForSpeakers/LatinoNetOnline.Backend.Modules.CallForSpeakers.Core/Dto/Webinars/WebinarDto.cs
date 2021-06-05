using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Webinars
{
    record WebinarDto(Guid Id, Guid ProposalId, Uri YoutubeLink, Uri MeetupLink, Uri FlyerLink);
}
