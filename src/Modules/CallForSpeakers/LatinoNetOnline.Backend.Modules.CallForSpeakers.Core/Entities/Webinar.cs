using LatinoNetOnline.Backend.Shared.Commons.Entities;

using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities
{
    public class Webinar : IEntity
    {
        public Webinar()
        {
        }

        public Webinar(Guid proposalId, Uri youtubeLink, Uri meetupLink, Uri flyerLink)
        {
            ProposalId = proposalId;
            YoutubeLink = youtubeLink;
            MeetupLink = meetupLink;
            FlyerLink = flyerLink;
        }

        public Guid Id { get; set; }
        public Guid ProposalId { get; set; }
        public Proposal Proposal { get; set; }
        public Uri YoutubeLink { get; set; }
        public Uri MeetupLink { get; set; }
        public Uri FlyerLink { get; set; }
    }
}
