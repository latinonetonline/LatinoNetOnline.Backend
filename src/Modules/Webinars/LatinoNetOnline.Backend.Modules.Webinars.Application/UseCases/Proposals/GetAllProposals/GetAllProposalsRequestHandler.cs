using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.GetAllProposals
{
    internal class GetAllProposalsRequestHandler : IRequestHandler<GetAllProposalsRequest, OperationResult<IEnumerable<ProposalFullDto>>>
    {
        private readonly IRepository<Proposal> _proposalRepository;

        public GetAllProposalsRequestHandler(IRepository<Proposal> proposalRepository)
        {
            _proposalRepository = proposalRepository;
        }

        public async Task<OperationResult<IEnumerable<ProposalFullDto>>> Handle(GetAllProposalsRequest request, CancellationToken cancellationToken)
        {


            var proposals = await _proposalRepository.Query(false).WhereIf(!string.IsNullOrWhiteSpace(request.Title), x => x.Title.Contains((request.Title ?? string.Empty).ToLower()))
                     .WhereIf(request.Date.HasValue, x => x.EventDate.Date == request.Date.GetValueOrDefault())
                     .WhereIf(request.IsActive.HasValue, x => x.IsActive == request.IsActive.GetValueOrDefault())
                     .WhereIf(request.Oldest.HasValue && request.Oldest.Value, x => x.EventDate.Date < DateTime.Today)
                     .WhereIf(!request.Oldest.HasValue || !request.Oldest.Value, x => x.EventDate.Date >= DateTime.Today)
                     .Include(x => x.Speakers)
                     .OrderByDescending(x => x.EventDate).ToListAsync();

            return OperationResult<IEnumerable<ProposalFullDto>>.Success(ConvertToFullDto(proposals));
        }

        private static ProposalFullDto ConvertToFullDto(Proposal proposal)
        {
            var proposalDto = proposal.ConvertToDto();
            var speakers = proposal.Speakers;
            var speakerDtos = speakers.Select(x => x.ConvertToDto());

            return new(proposalDto, speakerDtos);
        }

        private static IEnumerable<ProposalFullDto> ConvertToFullDto(IEnumerable<Proposal> proposals)
        {
            List<ProposalFullDto> proposalFullDtos = new();

            foreach (var proposal in proposals)
            {
                var proposalFullDto = ConvertToFullDto(proposal);
                proposalFullDtos.Add(proposalFullDto);
            }

            return proposalFullDtos;
        }
    }
}
