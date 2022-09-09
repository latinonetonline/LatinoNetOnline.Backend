using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using MediatR;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.CreateProposal
{
    internal record CreateProposalRequest(string Title, string Description, DateTime Date, string AudienceAnswer, string KnowledgeAnswer, string UseCaseAnswer, IEnumerable<CreateSpeakerInput> Speakers) : IRequest<OperationResult<ProposalFullDto>>;
}
