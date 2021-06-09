
using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Proposals
{
    record ProposalDto(Guid ProposalId, string Title, string Description, DateTime EventDate, DateTime CreationTime, string? AudienceAnswer, string? KnowledgeAnswer, string? UseCaseAnswer, bool IsActive);
}
