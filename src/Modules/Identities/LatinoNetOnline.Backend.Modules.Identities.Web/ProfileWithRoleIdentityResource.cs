using Duende.IdentityServer.Models;

using IdentityModel;

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
