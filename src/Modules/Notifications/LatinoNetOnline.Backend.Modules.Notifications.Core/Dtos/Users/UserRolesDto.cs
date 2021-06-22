using System;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Dtos.Users
{
    record UserRolesDto(Guid Id, string Name, string Lastname, string UserName, string Email, string Role);
}
