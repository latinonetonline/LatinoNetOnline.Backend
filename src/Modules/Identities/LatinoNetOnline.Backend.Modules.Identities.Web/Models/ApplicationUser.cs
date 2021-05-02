// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Identity;

using System;

namespace IdentityServerHost.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    class ApplicationUser : IdentityUser
    {
        private string? _name;


        public virtual string Name
        {
            set => _name = value;
            get => _name
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(_name));
        }

        private string? _lastname;


        public virtual string Lastname
        {
            set => _lastname = value;
            get => _lastname
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(_lastname));
        }
    }
}
