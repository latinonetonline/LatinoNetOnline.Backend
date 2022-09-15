using LatinoNetOnline.Backend.Modules.Webinars.Core.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.ChangeProposalFlyer
{
    public record ChangeProposalFlyerResponse(Guid ProposalId, string Title, string Description, DateTime EventDate, DateTime CreationTime, string? AudienceAnswer, string? KnowledgeAnswer, string? UseCaseAnswer, bool IsActive, int? WebinarNumber, WebinarStatus Status, Uri? Meetup, Uri? Streamyard, Uri? LiveStreaming, Uri? Flyer, int? Views, int? LiveAttends);
}