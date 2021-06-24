using LatinoNetOnline.Backend.Shared.Commons.Entities;

using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities
{
    public class Webinar : IEntity
    {
        public Webinar()
        {
        }

        public Webinar(Guid proposalId, long meetupId, Uri? liveStreaming, Uri? flyer)
        {
            ProposalId = proposalId;
            LiveStreaming = liveStreaming;
            MeetupId = meetupId;
            Flyer = flyer;
        }

        public Guid Id { get; set; }
        public Guid ProposalId { get; set; }
        public Proposal? Proposal { get; set; }

        public long MeetupId { get; set; }

        public Uri? LiveStreaming { get; set; }
        public Uri? Flyer { get; set; }
    }
}
