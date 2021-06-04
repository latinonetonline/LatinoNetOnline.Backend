using LatinoNetOnline.Backend.Shared.Commons.Entities;

using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities
{
    public class Proposal : IEntity, IHasCreationTime
    {
        public Proposal()
        {
        }

        public Proposal(string title, string description, string audienceAnswer, string knowledgeAnswer, string useCaseAnswer, DateTime eventDate)
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
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AudienceAnswer { get; set; }
        public string KnowledgeAnswer { get; set; }
        public string UseCaseAnswer { get; set; }
        public bool IsActive { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime CreationTime { get; set; }
        public ICollection<Speaker> Speakers { get; set; }


    }
}
