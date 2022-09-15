using LatinoNetOnline.Backend.Shared.Commons.Results;

using MediatR;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.GetProposalById
{
    public record GetProposalByIdRequest(Guid Id) : IRequest<Result<GetProposalByIdResponse>>;

}
