
using IdentityModel;

using IdentityServer4.Models;

namespace LatinoNetOnline.Backend.Modules.Identities.Web
{
    public class ProfileWithRoleIdentityResource : IdentityResources.Profile
    {
        public ProfileWithRoleIdentityResource()
        {
            UserClaims.Add(JwtClaimTypes.Role);
        }
    }
}
