using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.GetDatesOfProposals
{
    public class GetDatesOfProposalsRequestHandler : IRequestHandler<GetDatesOfProposalsRequest, Result<GetDatesOfProposalsResponse>>
    {
        private readonly IRepository<Proposal> _proposalRepository;
        private readonly IRepository<UnavailableDate> _unavailableDateRepository;
        public GetDatesOfProposalsRequestHandler(IRepository<Proposal> proposalRepository, IRepository<UnavailableDate> unavailableDateRepository)
        {
            _proposalRepository = proposalRepository;
            _unavailableDateRepository = unavailableDateRepository;
        }

        public async Task<Result<GetDatesOfProposalsResponse>> Handle(GetDatesOfProposalsRequest request, CancellationToken cancellationToken)
        {
            var dates = await _proposalRepository.Query(false)
            .Where(x => x.EventDate >= DateTime.Today)
            .Select(x => x.EventDate)
            .Union(_unavailableDateRepository.Query(false)
                .Where(x => x.Date >= DateTime.Today)
                .Select(x => x.Date))
            .ToListAsync();

            return new GetDatesOfProposalsResponse(dates.Select(x => DateOnly.FromDateTime(x)));
        }
    }
}
