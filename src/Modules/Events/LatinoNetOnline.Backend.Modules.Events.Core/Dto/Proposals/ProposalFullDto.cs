
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Proposals
{
    record ProposalFullDto(ProposalDto Proposal, IEnumerable<SpeakerDto> Speakers);
}
