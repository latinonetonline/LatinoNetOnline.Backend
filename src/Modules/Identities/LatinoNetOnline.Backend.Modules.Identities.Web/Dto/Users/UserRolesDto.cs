
using System;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Dto.Users
{
    record UserRolesDto(Guid Id, string Name, string Lastname, string UserName, string Email, string Role);
}
