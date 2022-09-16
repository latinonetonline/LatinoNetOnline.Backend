using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Speakers.GetAllSpeakers
{
    public record GetAllSpeakersResponse(IEnumerable<SpeakerDto> Speakers);
}