using LatinoNetOnline.Backend.Shared.Commons.Entities;

using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities
{
    public class Proposal : IEntity, IHasCreationTime
    {
        public Proposal()
        {
            Speakers = new HashSet<Speaker>();
        }

        public Proposal(string title, string description, string? audienceAnswer, string? knowledgeAnswer, string? useCaseAnswer, DateTime eventDate)
        {
            Title = title;
            Description = description;
            AudienceAnswer = audienceAnswer;
            KnowledgeAnswer = knowledgeAnswer;
            UseCaseAnswer = useCaseAnswer;
            EventDate = eventDate;
            CreationTime = DateTime.Now;
            Speakers = new List<Speaker>();
            IsActive = true;
            Speakers = new HashSet<Speaker>();
        }

        public Proposal(string title, string description, string audienceAnswer, string knowledgeAnswer, string useCaseAnswer, bool isActive, DateTime eventDate, DateTime creationTime, ICollection<Speaker> speakers)
        {
            Title = title;
            Description = description;
            AudienceAnswer = audienceAnswer;
            KnowledgeAnswer = knowledgeAnswer;
            UseCaseAnswer = useCaseAnswer;
            IsActive = isActive;
            EventDate = eventDate;
            CreationTime = creationTime;
            Speakers = speakers;
            Speakers = new HashSet<Speaker>();
        }

        public Guid Id { get; set; }

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


        public string? AudienceAnswer { get; set; }
        public string? KnowledgeAnswer { get; set; }
        public string? UseCaseAnswer { get; set; }
        public bool IsActive { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime CreationTime { get; set; }
        public ICollection<Speaker> Speakers { get; set; }


    }
}
