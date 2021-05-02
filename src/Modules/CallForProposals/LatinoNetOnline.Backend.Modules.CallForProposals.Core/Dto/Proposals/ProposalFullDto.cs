using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Speakers;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals
{
    record ProposalFullDto(ProposalDto Proposal, SpeakerDto Speaker);
}
