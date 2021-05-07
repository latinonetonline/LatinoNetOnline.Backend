using LatinoNetOnline.Backend.Shared.Abstractions.Entities;

using System;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities
{
    public class ProposalSpeaker : IEntity
    {
        public Guid Id { get; set; }
        public Guid ProposalId { get; set; }
        public virtual Proposal Proposal { get; set; }
        public Guid SpeakerId { get; set; }
        public virtual Speaker Speaker { get; set; }
    }
}
