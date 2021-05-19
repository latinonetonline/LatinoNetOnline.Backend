using LatinoNetOnline.Backend.Shared.Commons.Entities;

using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities
{
    public class Speaker : IEntity
    {
        public Speaker()
        {
        }

        public Speaker(string name, string lastName, string email, string twitter, string description, Uri image)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            Twitter = twitter;
            Description = description;
            Image = image;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Twitter { get; set; }
        public string Description { get; set; }
        public Uri Image { get; set; }

        public ICollection<Proposal> Proposals { get; set; }
    }
}
