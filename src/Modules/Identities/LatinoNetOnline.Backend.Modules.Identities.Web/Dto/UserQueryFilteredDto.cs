
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Dto
{
    record UserQueryFilteredDto(int Size, IEnumerable<UserRolesDto> Items);
}
