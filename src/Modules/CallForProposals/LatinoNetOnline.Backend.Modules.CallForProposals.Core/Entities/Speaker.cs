
using LatinoNetOnline.Backend.Shared.Abstractions.Entities;

using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities
{
    public class Speaker : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Twitter { get; set; }
        public string Description { get; set; }
        public Uri Image { get; set; }

        public virtual ICollection<Proposal> Proposals { get; set; }
    }
}
