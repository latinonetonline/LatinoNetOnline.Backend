using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Extensions
{
    static class WebinarExtensions
    {
        public static Webinar ConvertToEntity(this CreateWebinarInput input)
            => new(input.ProposalId, input.YoutubeLink, input.MeetupLink, input.FlyerLink);

        public static WebinarDto ConvertToDto(this Webinar webinar)
            => new(webinar.Id, webinar.ProposalId, webinar.YoutubeLink, webinar.MeetupLink, webinar.FlyerLink);
    }
}
