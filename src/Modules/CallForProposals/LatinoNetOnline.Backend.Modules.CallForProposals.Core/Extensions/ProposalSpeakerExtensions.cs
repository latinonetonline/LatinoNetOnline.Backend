using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.ProposalSpeakers;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Extensions
{
    static class ProposalSpeakerExtensions
    {
        public static ProposalSpeakerDto ConvertToDto(this ProposalSpeaker proposalSpeaker)
            => new(proposalSpeaker.Id, proposalSpeaker.SpeakerId, proposalSpeaker.ProposalId);
    }
}
