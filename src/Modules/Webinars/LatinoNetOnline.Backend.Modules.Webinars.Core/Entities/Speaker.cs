using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Emails;
using LatinoNetOnline.Backend.Shared.Commons.Entities;

using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Entities
{
    public class Speaker : IEntity
    {
        public Speaker(string name, string lastName, string email, string? twitter, string description, Uri image, Guid userId)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            Twitter = twitter;
            Description = description;
            Image = image;
            UserId = userId;

            Proposals = new HashSet<Proposal>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }


        public string Email { get; set; }

        public string Description { get; set; }

        public string? Twitter { get; set; }

        public Uri Image { get; set; }

        public Guid UserId { get; set; }

        public ICollection<Proposal> Proposals { get; set; }
    }
}
