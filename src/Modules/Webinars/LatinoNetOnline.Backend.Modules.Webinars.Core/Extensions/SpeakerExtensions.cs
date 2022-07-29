using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Extensions
{
    static class SpeakerExtensions
    {
        public static SpeakerDto ConvertToDto(this Speaker speaker)
            => new(speaker.Id, speaker.Name, speaker.LastName, speaker.Email, speaker.Twitter, speaker.Description, speaker.Image);
    }
}
