using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.GetProposalById
{
    public class GetProposalByIdRequestHandler : IRequestHandler<GetProposalByIdRequest, Result<GetProposalByIdResponse>>
    {
        private readonly IRepository<Proposal> _repository;
        public GetProposalByIdRequestHandler(IRepository<Proposal> repository)
        {
            _repository = repository;
        }

        public async Task<Result<GetProposalByIdResponse>> Handle(GetProposalByIdRequest request, CancellationToken cancellationToken)
        {
            var proposal = await _repository.SingleOrDefaultAsync(x => x.Id == request.Id, false, cancellationToken);

            if (proposal is null)
            {
                return "No hay ninguna propuesta.";
            }

            return new GetProposalByIdResponse(proposal.ConvertToDto(), proposal.Speakers.Select(x => x.ConvertToDto()));
        }
    }
}
