using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Shared.Commons.Results;

using MediatR;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.UpdateProposal
{
    public record UpdateProposalRequest(Guid Id, string Title, string Description, DateTime Date, string AudienceAnswer, string KnowledgeAnswer, string UseCaseAnswer, int? WebinarNumber, Uri? Meetup, Uri? Streamyard, Uri? LiveStreaming, Uri? Flyer, int? Views, int? LiveAttends, IEnumerable<UpdateSpeakerInput> Speakers) : IRequest<Result<UpdateProposalResponse>>;

}
