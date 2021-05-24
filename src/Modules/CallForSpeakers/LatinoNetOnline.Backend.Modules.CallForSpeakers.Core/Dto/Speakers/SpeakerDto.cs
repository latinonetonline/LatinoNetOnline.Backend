
using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers
{
    record SpeakerDto(Guid SpeakerId, string Name, string LastName, string Email, string Twitter, string Description, Uri Image);
}
