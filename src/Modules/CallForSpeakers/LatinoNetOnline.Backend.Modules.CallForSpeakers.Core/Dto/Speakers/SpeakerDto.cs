
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Emails;

using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers
{
    record SpeakerDto(Guid SpeakerId, string Name, string LastName, Email Email, string? Twitter, string Description, Uri Image);
}
