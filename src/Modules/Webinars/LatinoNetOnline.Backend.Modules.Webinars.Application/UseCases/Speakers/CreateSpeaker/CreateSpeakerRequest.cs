using LatinoNetOnline.Backend.Shared.Commons.Results;

using MediatR;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Speakers.CreateSpeaker
{
    public record CreateSpeakerRequest(string Name, string LastName, string Email, string? Twitter, string Description, Uri Image) : IRequest<Result<CreateSpeakerResponse>>;

}
