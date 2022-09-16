using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions;
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

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Speakers.GetAllSpeakers
{
    public class GetAllSpeakersRequestHandler : IRequestHandler<GetAllSpeakersRequest, Result<GetAllSpeakersResponse>>
    {
        private readonly IRepository<Proposal> _repository;

        public GetAllSpeakersRequestHandler(IRepository<Proposal> repository)
        {
            _repository = repository;
        }

        public async Task<Result<GetAllSpeakersResponse>> Handle(GetAllSpeakersRequest request, CancellationToken cancellationToken)
        {

            List<Speaker> speakers = new();


            var speakersMany = await _repository.Query(false)
               .Include(x => x.Speakers)
               .OrderByDescending(x => x.CreationTime)
               .Select(x => x.Speakers)
               .ToListAsync(cancellationToken);

            foreach (var speakers1 in speakersMany)
            {
                foreach (var item in speakers1)
                {
                    if (!speakers.Any(x => x.Id == item.Id))
                        speakers.Add(item);
                }
            }

            return new GetAllSpeakersResponse(speakers.Select(x => x.ConvertToDto()));
        }
    }
}
