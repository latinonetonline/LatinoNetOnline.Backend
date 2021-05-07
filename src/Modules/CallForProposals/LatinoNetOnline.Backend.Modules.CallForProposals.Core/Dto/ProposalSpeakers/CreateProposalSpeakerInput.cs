using System;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.ProposalSpeakers
{
    record CreateProposalSpeakerInput(Guid SpeakerId, Guid ProposalId);
}
