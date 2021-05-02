using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Speakers;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Extensions
{
    static class SpeakerExtensions
    {
        public static SpeakerDto ConvertToDto(this Speaker speaker)
            => new(speaker.Id, speaker.Name, speaker.LastName, speaker.Email, speaker.Twitter, speaker.Description, speaker.Image);
    }
}
