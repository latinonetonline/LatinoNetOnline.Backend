
using LatinoNetOnline.Backend.Modules.Events.Core.Enums;

using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Entities
{
    public class Webinar
    {
        public Webinar()
        {
        }

        public Webinar(Guid proposalId, int number, long meetupId, Uri? streamyard, Uri? liveStreaming, Uri? flyer)
        {
            ProposalId = proposalId;
            LiveStreaming = liveStreaming;
            Streamyard = streamyard;
            MeetupId = meetupId;
            Number = number;
            Flyer = flyer;
        }

        public Guid Id { get; set; }
        public Guid ProposalId { get; set; }

        private Proposal? _proposal;

        public Proposal Proposal
        {
            set => _proposal = value;
            get => _proposal
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Proposal));
        }

        public int Number { get; set; }
        public long MeetupId { get; set; }
        public Uri? Streamyard { get; set; }
        public Uri? LiveStreaming { get; set; }
        public Uri? Flyer { get; set; }

        public WebinarStatus Status { get; set; }
    }
}
