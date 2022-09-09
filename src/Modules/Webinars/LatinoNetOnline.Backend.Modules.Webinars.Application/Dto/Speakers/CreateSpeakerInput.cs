
using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers
{
    record CreateSpeakerInput(string Name, string LastName, string Email, string? Twitter, string Description, Uri Image);
}
