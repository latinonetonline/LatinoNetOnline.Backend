using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.DeleteProposal
{
    public class DeleteProposalRequestHandler : IRequestHandler<DeleteProposalRequest, Result>
    {
        private readonly IRepository<Proposal> _repository;

        public DeleteProposalRequestHandler(IRepository<Proposal> repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteProposalRequest request, CancellationToken cancellationToken)
        {
            var proposal = await _repository.SingleOrDefaultAsync(x => x.Id == request.Id, true, cancellationToken);

            if (proposal is null)
            {
                return "No hay ninguna propuesta.";
            }

            await _repository.RemoveAsync(proposal, cancellationToken);

            return true;
        }
    }
}
