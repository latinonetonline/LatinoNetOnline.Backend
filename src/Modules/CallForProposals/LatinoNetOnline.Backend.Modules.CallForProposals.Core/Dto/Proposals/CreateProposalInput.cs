
using System;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals
{
    public record CreateProposalInput(Guid SpeakerId, string Title, string Description, DateTime Date, string AudienceAnswer, string KnowledgeAnswer, string UseCaseAnswer);
}
