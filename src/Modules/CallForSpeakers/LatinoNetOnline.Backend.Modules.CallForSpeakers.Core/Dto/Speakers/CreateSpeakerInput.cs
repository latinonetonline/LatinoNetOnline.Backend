using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers
{
    record CreateSpeakerInput(string Name, string LastName, string Email, string Twitter, string Description, Uri Image);
}
