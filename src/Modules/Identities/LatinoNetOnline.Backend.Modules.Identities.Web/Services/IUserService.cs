
using IdentityServerHost.Models;

using Microsoft.AspNetCore.Identity;

using LatinoNetOnline.Backend.Modules.Identities.Web.Dto;
using LatinoNetOnline.Backend.Shared.Abstractions.OperationResults;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Services
{
    interface IUserService
    {
        Task<OperationResult<ApplicationUser>> GetByIdAsync(string id);
        Task<OperationResult<IEnumerable<ApplicationUser>>> GetAllAsync();
        Task<OperationResult<IEnumerable<IdentityRole>>> GetAllRolesAsync();
        Task<OperationResult<UserQueryFilteredDto>> GetAllAsync(QueryFiltersDto filters);
        Task<OperationResult<UserRolesDto>> GetFullByIdAsync(string id);
        Task<OperationResult> DeleteAsync(string userId);
        Task<OperationResult<UserRolesDto>> EditAsync(UserRolesDto user);
        Task<OperationResult<UserRolesDto>> CreateAsync(CreateUserInput input);
        Task<OperationResult> AsignRoleToUser(ApplicationUser userRoles, string role);
    }
}
