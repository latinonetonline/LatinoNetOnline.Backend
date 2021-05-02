using System;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Options
{
    class IdentityOptions
    {
        public string? FrontendUrl { get; set; }
        public string? ClientSecret { get; set; }
        public Uri? UserImageUrl { get; set; }
        public string? TermsAndConditionVersion { get; set; }
    }
}
