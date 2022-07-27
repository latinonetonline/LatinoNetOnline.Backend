using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Emails;
using LatinoNetOnline.Backend.Shared.Commons.Entities;

using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Entities
{
    public class Speaker : IEntity
    {
        public Speaker()
        {
            Proposals = new HashSet<Proposal>();
        }

        public Speaker(string name, string lastName, Email email, string? twitter, string description, Uri image)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            Twitter = twitter;
            Description = description;
            Image = image;

            Proposals = new HashSet<Proposal>();
        }

        public Guid Id { get; set; }

        private string? _name;

        public string Name
        {
            set => _name = value;
            get => _name
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Name));
        }

        private string? _lastName;

        public string LastName
        {
            set => _lastName = value;
            get => _lastName
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(LastName));
        }


        private Email? _email;

        public Email Email
        {
            set => _email = value;
            get => _email
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Email));
        }

        private string? _description;

        public string Description
        {
            set => _description = value;
            get => _description
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Description));
        }

        public string? Twitter { get; set; }

        private Uri? _image;

        public Uri Image
        {
            set => _image = value;
            get => _image
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Image));
        }

        public ICollection<Proposal> Proposals { get; set; }
    }
}
