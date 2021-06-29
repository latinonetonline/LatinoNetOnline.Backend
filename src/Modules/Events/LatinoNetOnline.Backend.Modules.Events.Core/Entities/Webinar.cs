
using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Entities
{
    public class Webinar
    {
        public Webinar()
        {
        }

        public Webinar(Guid proposalId, string title, string description, long meetupId, DateTime startDateTime, Uri? liveStreaming, Uri? flyer)
        {
            ProposalId = proposalId;
            LiveStreaming = liveStreaming;
            MeetupId = meetupId;
            Flyer = flyer;
            StartDateTime = startDateTime;
            _title = title;
            _description = description;
        }

        public Guid Id { get; set; }
        public Guid ProposalId { get; set; }

        private string? _title;

        public string Title
        {
            set => _title = value;
            get => _title
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Title));
        }

        private string? _description;

        public string Description
        {
            set => _description = value;
            get => _description
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Description));
        }


        public long MeetupId { get; set; }
        public DateTime StartDateTime { get; set; }
        public Uri? LiveStreaming { get; set; }
        public Uri? Flyer { get; set; }
    }
}
