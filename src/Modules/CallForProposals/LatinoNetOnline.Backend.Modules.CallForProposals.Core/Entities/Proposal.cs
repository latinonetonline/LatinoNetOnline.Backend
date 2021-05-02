
using LatinoNetOnline.Backend.Shared.Abstractions.Entities;

using System;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities
{
    public class Proposal : IEntity, IHasCreationTime
    {
        public Guid Id { get; set; }
        public Guid SpeakerId { get; set; }
        public Speaker Speaker { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string AudienceAnswer { get; set; }
        public string KnowledgeAnswer { get; set; }
        public string UseCaseAnswer { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
