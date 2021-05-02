
using System;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Speakers
{
    record SpeakerDto(Guid SpeakerId, string Name, string LastName, string Email, string Twitter, string Description, Uri Image);
}
