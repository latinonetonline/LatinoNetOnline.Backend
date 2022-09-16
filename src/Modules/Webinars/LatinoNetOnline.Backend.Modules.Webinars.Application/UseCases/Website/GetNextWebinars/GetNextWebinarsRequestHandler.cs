using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Enums;
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

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Website.GetNextWebinars
{
    public class GetNextWebinarsRequestHandler : IRequestHandler<GetNextWebinarsRequest, Result<GetNextWebinarsResponse?>>
    {
        private readonly IRepository<Proposal> _repository;

        public GetNextWebinarsRequestHandler(IRepository<Proposal> repository)
        {
            _repository = repository;
        }

        public async Task<Result<GetNextWebinarsResponse?>> Handle(GetNextWebinarsRequest request, CancellationToken cancellationToken)
        {
            var proposal = await _repository.Query(false)
                .Where(x => x.Status == WebinarStatus.Published && x.EventDate.Date >= DateTime.Today)
                .OrderByDescending(x => x.EventDate)
                .Select(x => new GetNextWebinarsResponse(x.Title, x.Description, x.Flyer!, x.LiveStreaming!, x.Meetup!, x.EventDate))
                .FirstOrDefaultAsync();


            return proposal;
        }
    }
}
