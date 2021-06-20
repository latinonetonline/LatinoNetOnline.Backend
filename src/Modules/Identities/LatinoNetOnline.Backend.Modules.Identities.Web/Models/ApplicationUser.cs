// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Identity;

namespace IdentityServerHost.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public string Lastname { get; set; }
    }
}
