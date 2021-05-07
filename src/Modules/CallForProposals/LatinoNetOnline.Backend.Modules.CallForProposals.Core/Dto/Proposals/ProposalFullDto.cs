using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Speakers;

using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals
{
    record ProposalFullDto(ProposalDto Proposal, IEnumerable<SpeakerDto> Speakers);
}
