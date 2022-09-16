using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Speakers.CreateSpeaker
{
    public record CreateSpeakerResponse(Guid SpeakerId, string Name, string LastName, string Email, string? Twitter, string Description, Uri Image);
}