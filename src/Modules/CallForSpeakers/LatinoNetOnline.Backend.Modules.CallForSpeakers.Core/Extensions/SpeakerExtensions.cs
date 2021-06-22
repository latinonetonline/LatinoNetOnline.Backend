using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Extensions
{
    static class SpeakerExtensions
    {
        public static SpeakerDto ConvertToDto(this Speaker speaker)
            => new(speaker.Id, speaker.Name, speaker.LastName, speaker.Email.ToString(), speaker.Twitter, speaker.Description, speaker.Image);
    }
}
