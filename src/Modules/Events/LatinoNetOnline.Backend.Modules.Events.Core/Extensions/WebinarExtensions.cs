using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Extensions
{
    static class WebinarExtensions
    {
        public static Webinar ConvertToEntity(this CreateWebinarInput input)
            => new(input.ProposalId, input.Number, 0, input.StartDateTime, null, null, null);

        public static WebinarDto ConvertToDto(this Webinar webinar)
            => new(webinar.Id, webinar.ProposalId, webinar.Number, webinar.MeetupId, webinar.StartDateTime, webinar.Streamyard, webinar.LiveStreaming, webinar.Flyer, webinar.Status);

        public static UpdateWebinarInput ConvertToUpdateInput(this WebinarDto webinar)
            => new(webinar.Id, webinar.Number, webinar.MeetupId, webinar.StartDateTime, webinar.Streamyard, webinar.LiveStreaming, webinar.Flyer, webinar.Status);


        public static Task<int?> MaxNumberAsync(this IQueryable<Webinar> query)
            => query.AsNoTracking().MaxAsync(x => (int?)x.Number);
    }
}
