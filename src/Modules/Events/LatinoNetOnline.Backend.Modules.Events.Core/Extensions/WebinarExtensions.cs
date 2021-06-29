using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Extensions
{
    static class WebinarExtensions
    {
        public static Webinar ConvertToEntity(this CreateWebinarInput input)
            => new(input.ProposalId, input.Title, input.Description, input.MeetupId, input.StartDateTime, null, null);

        public static WebinarDto ConvertToDto(this Webinar webinar)
            => new(webinar.Id, webinar.ProposalId, webinar.Title, webinar.Description, webinar.MeetupId, webinar.StartDateTime, webinar.LiveStreaming, webinar.Flyer);
    }
}
