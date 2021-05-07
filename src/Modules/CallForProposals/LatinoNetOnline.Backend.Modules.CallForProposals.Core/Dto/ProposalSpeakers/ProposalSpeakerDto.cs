using System;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.ProposalSpeakers
{
    record ProposalSpeakerDto(Guid Id, Guid SpeakerId, Guid ProposalId);
}
