
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Dtos.Users
{
    record UserQueryFilteredDto(int Size, IEnumerable<UserRolesDto> Items);
}
