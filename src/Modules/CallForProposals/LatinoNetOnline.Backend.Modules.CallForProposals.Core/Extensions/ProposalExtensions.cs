using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Extensions
{
    static class ProposalExtensions
    {
        public static ProposalDto ConvertToDto(this Proposal proposal)
            => new(proposal.Id, proposal.Title, proposal.Description, proposal.EventDate, proposal.CreationTime, proposal.AudienceAnswer, proposal.KnowledgeAnswer, proposal.UseCaseAnswer);

        public static ProposalFullDto ConvertToFullDto(this Proposal proposal)
            => new(proposal.ConvertToDto(), proposal.Speaker.ConvertToDto());
    }
}
