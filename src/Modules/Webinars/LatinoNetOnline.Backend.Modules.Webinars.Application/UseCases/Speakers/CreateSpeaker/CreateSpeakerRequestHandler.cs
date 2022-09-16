using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Speakers.CreateSpeaker
{
    public class CreateSpeakerRequestHandler : IRequestHandler<CreateSpeakerRequest, Result<CreateSpeakerResponse>>
    {
        private readonly IRepository<Speaker> _repository;


        public CreateSpeakerRequestHandler(IRepository<Speaker> repository)
        {
            _repository = repository;
        }

        public async Task<Result<CreateSpeakerResponse>> Handle(CreateSpeakerRequest request, CancellationToken cancellationToken)
        {
           Speaker speaker =  new(request.Name, request.LastName, new(request.Email), request.Twitter, request.Description, request.Image, Guid.Empty);

            await _repository.AddAsync(speaker, cancellationToken);

            CreateSpeakerResponse response = new(speaker.Id, speaker.Name, speaker.LastName, speaker.Email, speaker.Twitter, speaker.Description, speaker.Image);


            return response;
        }
    }
}
