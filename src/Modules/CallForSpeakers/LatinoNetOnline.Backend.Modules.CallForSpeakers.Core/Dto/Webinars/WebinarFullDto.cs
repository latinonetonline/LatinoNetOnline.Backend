using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers;

using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Webinars
{
    record WebinarFullDto(WebinarDto Webinar, ProposalDto Proposal, IEnumerable<SpeakerDto> Speakers);
}
