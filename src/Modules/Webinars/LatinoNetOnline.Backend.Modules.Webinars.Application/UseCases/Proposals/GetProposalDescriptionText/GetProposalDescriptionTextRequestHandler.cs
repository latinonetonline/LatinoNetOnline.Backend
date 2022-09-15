using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.GetProposalDescriptionText
{
    public class GetProposalDescriptionTextRequestHandler : IRequestHandler<GetProposalDescriptionTextRequest, Result<GetProposalDescriptionTextResponse>>
    {
        private readonly IRepository<Proposal> _repository;

        public GetProposalDescriptionTextRequestHandler(IRepository<Proposal> repository)
        {
            _repository = repository;
        }

        public async Task<Result<GetProposalDescriptionTextResponse>> Handle(GetProposalDescriptionTextRequest request, CancellationToken cancellationToken)
        {
            var proposal = await _repository.Query(true).Include(x => x.Speakers).SingleOrDefaultAsync(x => x.Id == request.Id);

            if (proposal is null)
            {
                return "No hay ninguna propuesta.";
            }

            ProposalFullDto proposalFullDto = new(proposal.ConvertToDto(), proposal.Speakers.Select(x => x.ConvertToDto()));

            return new GetProposalDescriptionTextResponse(proposalFullDto.GetDescription());
        }
    }
}
