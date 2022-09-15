
using LatinoNetOnline.Backend.Modules.Webinars.Core.Enums;

using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals
{
    public record ProposalDto(Guid ProposalId, string Title, string Description, DateTime EventDate, DateTime CreationTime, string? AudienceAnswer, string? KnowledgeAnswer, string? UseCaseAnswer, bool IsActive, int? WebinarNumber, WebinarStatus Status, Uri? Meetup, Uri? Streamyard, Uri? LiveStreaming, Uri? Flyer, int? Views, int? LiveAttends);
}
