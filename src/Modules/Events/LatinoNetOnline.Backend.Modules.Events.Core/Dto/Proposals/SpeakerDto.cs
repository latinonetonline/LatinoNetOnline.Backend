
using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Proposals
{
    record SpeakerDto(Guid SpeakerId, string Name, string LastName, string Email, string? Twitter, string Description, Uri Image);
}
