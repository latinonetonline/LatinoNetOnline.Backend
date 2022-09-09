using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;

using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals
{
    record ProposalFullDto(ProposalDto Proposal, IEnumerable<SpeakerDto> Speakers);
}
