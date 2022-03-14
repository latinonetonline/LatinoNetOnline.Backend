using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Extensions
{
    internal static class SpeakerExtensions
    {
        public static SpeakerDto ConvertToDto(this Speaker speaker)
            => new(speaker.Id, speaker.Name, speaker.LastName, speaker.Email.ToString(), speaker.Twitter, speaker.Description, speaker.Image);
    }
}
