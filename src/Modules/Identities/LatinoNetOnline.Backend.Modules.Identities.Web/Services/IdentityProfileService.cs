using IdentityModel;

using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

using IdentityServerHost.Models;

using LatinoNetOnline.Backend.Modules.Identities.Web.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

#nullable enable
namespace LatinoNetOnline.Backend.Modules.Identities.Web.Services
{
    class IdentityProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            List<Claim> newClaims = new ();
            ApplicationUser? user = default;

            string? idUser = context.Subject.FindFirst(JwtClaimTypes.Subject)?.Value;

            if (string.IsNullOrWhiteSpace(idUser))
            {
                string? email = context.Subject.FindFirst(JwtClaimTypes.Email)?.Value;
                user = await _userManager.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            }
            else
            {
                user = await _userManager.Users.Where(x => x.Id == idUser).FirstOrDefaultAsync();
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);

            foreach (var roleName in roles)
            {
                newClaims.Add(new Claim(JwtClaimTypes.Role, roleName));
            }

            newClaims.Add(new Claim(JwtClaimTypes.Name, user.Name));
            newClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));

            context.IssuedClaims.AddRange(newClaims);
            context.AddRequestedClaims(newClaims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
