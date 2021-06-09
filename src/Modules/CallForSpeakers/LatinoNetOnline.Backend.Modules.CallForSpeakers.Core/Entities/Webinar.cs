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
        public Proposal? Proposal { get; set; }

        private Uri? _youtubeLink;

        public Uri YoutubeLink
        {
            set => _youtubeLink = value;
            get => _youtubeLink
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(YoutubeLink));
        }


        private Uri? _meetupLink;

        public Uri MeetupLink
        {
            set => _meetupLink = value;
            get => _meetupLink
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(MeetupLink));
        }


        private Uri? _flyerLink;

        public Uri FlyerLink
        {
            set => _flyerLink = value;
            get => _flyerLink
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(FlyerLink));
        }
    }
}
