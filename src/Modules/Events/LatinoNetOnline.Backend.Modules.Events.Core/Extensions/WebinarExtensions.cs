using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Entities;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Extensions
{
    static class WebinarExtensions
    {
        public static WebinarDto ConvertToDto(this Webinar webinar)
            => new(webinar.Id, webinar.ProposalId, webinar.Number, webinar.MeetupId, webinar.Streamyard, webinar.LiveStreaming, webinar.Flyer, webinar.Status);

        public static UpdateWebinarInput ConvertToUpdateInput(this WebinarDto webinar)
            => new(webinar.Id, webinar.MeetupId, webinar.Streamyard, webinar.LiveStreaming, webinar.Flyer, webinar.Status);


        public static Task<int?> MaxNumberAsync(this IQueryable<Webinar> query)
            => query.AsNoTracking().MaxAsync(x => (int?)x.Number);

        public static void UpdateWebinarNumber(this IEnumerable<Webinar> webinars, int lastWebinarConfirmated)
        {
            var webinarsList = webinars.OrderBy(x => x.Proposal.EventDate).ToList();

            for (int i = 0; i < webinarsList.Count; i++)
            {
                var webinar = webinarsList[i];

                int nextWebinarNumber = lastWebinarConfirmated + i + 1;

                webinar.Number = nextWebinarNumber;
            }
        }
    }
}
