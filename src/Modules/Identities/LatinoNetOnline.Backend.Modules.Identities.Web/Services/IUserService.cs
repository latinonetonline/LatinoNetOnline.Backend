
using LatinoNetOnline.Backend.Modules.Identities.Web.Dto.Roles;
using LatinoNetOnline.Backend.Modules.Identities.Web.Dto.Users;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Services
{
    interface IUserService
    {
        Task<OperationResult<IEnumerable<RoleDto>>> GetAllRolesAsync();
        Task<OperationResult<IEnumerable<UserRolesDto>>> GetAllAsync(GetAllUserInput filters);
        Task<OperationResult<UserRolesDto>> GetFullByIdAsync(string id);
        Task<OperationResult> DeleteAsync(string userId);
        Task<OperationResult<UserRolesDto>> EditAsync(UserRolesDto user);
    }
}
