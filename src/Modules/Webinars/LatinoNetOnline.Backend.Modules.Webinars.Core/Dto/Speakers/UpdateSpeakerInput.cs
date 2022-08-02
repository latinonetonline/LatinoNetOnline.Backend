using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers
{
    record UpdateSpeakerInput(Guid Id, string Name, string LastName, string Email, string? Twitter, string Description, Uri Image);
}
