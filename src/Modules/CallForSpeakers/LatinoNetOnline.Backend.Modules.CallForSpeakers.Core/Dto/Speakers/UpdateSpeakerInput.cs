using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers
{
    record UpdateSpeakerInput(Guid Id, string Name, string LastName, string Email, string? Twitter, string Description);
}
