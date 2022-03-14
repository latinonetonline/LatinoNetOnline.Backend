
using IdentityServerHost.Models;

using LatinoNetOnline.Backend.Modules.Identities.Web.Data;
using LatinoNetOnline.Backend.Modules.Identities.Web.Dto.Roles;
using LatinoNetOnline.Backend.Modules.Identities.Web.Dto.Users;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Services
{
    class UserService : IUserService
    {
        #region Attributes
        private readonly UserManager<ApplicationUser> _context;
        private readonly ApplicationDbContext _applicationContext;
        private readonly RoleManager<IdentityRole> _contextRole;
        #endregion

        #region Constructor
        public UserService(UserManager<ApplicationUser> context, RoleManager<IdentityRole> contextRole, ApplicationDbContext applicationContext)
        {
            _applicationContext = applicationContext;
            _context = context;
            _contextRole = contextRole;
        }
        #endregion

        #region MethodPublics

        public async Task<OperationResult<IEnumerable<RoleDto>>> GetAllRolesAsync()
        {
            var roles = await _contextRole.Roles.ToListAsync();

            var rolesDto = roles.Select(x => new RoleDto(Guid.Parse(x.Id), x.Name));

            return OperationResult<IEnumerable<RoleDto>>.Success(rolesDto);
        }

        public async Task<OperationResult<UserRolesDto>> GetFullByIdAsync(string id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user is null)
            {
                return OperationResult<UserRolesDto>.NotFound();
            }
            else
            {
                var getRolById = await _context.GetRolesAsync(user);
                UserRolesDto usersItems = ConvertToDto(user, getRolById.First());

                return OperationResult<UserRolesDto>.Success(usersItems);
            }
        }
        public async Task<OperationResult<IEnumerable<UserRolesDto>>> GetAllAsync(GetAllUserInput filters)
        {

            var queryUsers = _context.Users.AsNoTracking();
            var queryRoles = _contextRole.Roles.AsNoTracking();

            var size = await queryUsers.CountAsync();

            if (!string.IsNullOrWhiteSpace(filters.Search))
            {
                queryUsers = queryUsers.Where(x => x.Name.ToLower().Contains(filters.Search.ToLower()) || x.Lastname.ToLower().Contains(filters.Search.ToLower()) || x.NormalizedEmail.Contains(filters.Search.ToUpper()));
            }

            if (filters.Users?.Any() ?? false)
            {
                queryUsers = queryUsers.Where(x => filters.Users.Contains(x.Id));
            }

            if (!string.IsNullOrWhiteSpace(filters.Role))
            {
                queryRoles = queryRoles.Where(x => x.Name.ToLower().Contains(filters.Role.ToLower()));
            }


            // var users = await query.ToListAsync();

            var items = await (from r in _applicationContext.UserRoles.AsNoTracking()
                               join us in queryUsers on r.UserId equals us.Id
                               join ru in queryRoles on r.RoleId equals ru.Id
                               select ConvertToDto(us, ru.Name)).ToListAsync();

            return OperationResult<IEnumerable<UserRolesDto>>.Success(items);

        }

        public async Task<OperationResult<UserRolesDto>> EditAsync(UserRolesDto user)
        {

            var getUser = _context.FindByIdAsync(user.Id.ToString()).Result;

            getUser.Email = user.Email;
            getUser.Name = user.Name;
            getUser.Lastname = user.Lastname;
            getUser.UserName = user.UserName;

            var response = await _context.UpdateAsync(getUser);

            if (!response.Succeeded)
            {
                return OperationResult<UserRolesDto>.Fail(new("error_edit_user"));
            }

            var asignRoleToUser = await AsignRoleToUser(getUser, user.Role);
            var userById = await _context.Users.SingleAsync(x => x.Id == user.Id.ToString());

            UserRolesDto resultView = ConvertToDto(userById, user.Role);
            return OperationResult<UserRolesDto>.Success(resultView);

        }
        public async Task<OperationResult> DeleteAsync(string userId)
        {
            var findUserById = await _context.FindByIdAsync(userId);

            if (findUserById is null)
            {
                return OperationResult<UserRolesDto>.NotFound();
            }

            var roleByUser = await _applicationContext.UserRoles.SingleAsync(x => x.UserId == userId);
            _applicationContext.UserRoles.Remove(roleByUser);

            var result = await _context.DeleteAsync(findUserById);

            if (!result.Succeeded)
            {
                return OperationResult<UserRolesDto>.Fail(new("error_delete_user"));
            }

            return OperationResult.Success();

        }

        #endregion

        #region MethodsPrivate
        private async Task<OperationResult> AsignRoleToUser(ApplicationUser user, string role)
        {
            var findUserById = await _context.Users.SingleOrDefaultAsync(x => x.Id == user.Id);

            if (findUserById is null)
            {
                return OperationResult.Fail();
            }

            var rolesByUser = await _context.GetRolesAsync(findUserById);
            if (rolesByUser.Count > 0)
            {
                await _context.RemoveFromRoleAsync(findUserById, rolesByUser.Single());
            }

            var result = await _context.AddToRoleAsync(findUserById, role);

            if (!result.Succeeded)
            {
                return OperationResult.Fail();
            }

            return OperationResult.Success();

        }


        static UserRolesDto ConvertToDto(ApplicationUser user, string role)
            => new(Guid.Parse(user.Id), user.Name, user.Lastname, user.UserName, user.Email, role);


        #endregion


    }
}
