using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers;

using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Proposals
{
    record ProposalFullDto(ProposalDto Proposal, IEnumerable<SpeakerDto> Speakers);
}
