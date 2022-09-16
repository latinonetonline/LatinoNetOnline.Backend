using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Enums;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Website.GetPastWebinars
{
    public class GetPastWebinarsRequestHandler : IRequestHandler<GetPastWebinarsRequest, Result<GetPastWebinarsResponse>>
    {
        private readonly IRepository<Proposal> _repository;


        public GetPastWebinarsRequestHandler(IRepository<Proposal> repository)
        {
            _repository = repository;
        }

        public async Task<Result<GetPastWebinarsResponse>> Handle(GetPastWebinarsRequest request, CancellationToken cancellationToken)
        {
            var proposals = await _repository.Query(false)
                .Where(x => x.Status == WebinarStatus.Published && x.EventDate.Date < DateTime.Today)
                .OrderByDescending(x => x.EventDate)
                .Take(3)
                .Select(x => new ProposalPublicDto(x.Title, x.Description, x.Flyer!, x.LiveStreaming!, x.Meetup!, x.EventDate))
                .ToListAsync();

            return new GetPastWebinarsResponse(proposals);
        }
    }
}
