using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.DeleteAllProposals
{
    public class DeleteAllProposalsRequestHandler : IRequestHandler<DeleteAllProposalsRequest, Result>
    {

        private readonly IRepository<Proposal> _repository;
        public DeleteAllProposalsRequestHandler(IRepository<Proposal> repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteAllProposalsRequest request, CancellationToken cancellationToken)
        {
            await _repository.RemoveRangeAsync(_repository.Query(true), cancellationToken);

            return true;
        }
    }
}
