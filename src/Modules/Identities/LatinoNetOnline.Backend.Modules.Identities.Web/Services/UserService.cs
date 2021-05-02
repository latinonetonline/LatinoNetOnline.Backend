
using IdentityModel;

using IdentityServerHost.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using LatinoNetOnline.Backend.Modules.Identities.Web.Data;
using LatinoNetOnline.Backend.Modules.Identities.Web.Dto;
using LatinoNetOnline.Backend.Shared.Abstractions.OperationResults;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public async Task<OperationResult<ApplicationUser>> GetByIdAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user is null)
                return OperationResult<ApplicationUser>.Fail(new("user_not_found"));

            return OperationResult<ApplicationUser>.Success(user);
        }

        public async Task<OperationResult<IEnumerable<ApplicationUser>>> GetAllAsync()
        {
            var result = await _context.Users.ToListAsync();
            return OperationResult<IEnumerable<ApplicationUser>>.Success(result);
        }

        public async Task<OperationResult<IEnumerable<IdentityRole>>> GetAllRolesAsync()
        {
            var result = await _contextRole.Roles.ToListAsync();
            return OperationResult<IEnumerable<IdentityRole>>.Success(result);
        }

        public async Task<OperationResult<UserRolesDto>> GetFullByIdAsync(string id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user is null)
                return OperationResult<UserRolesDto>.NotFound();
            else
            {
                var getRolById = await _context.GetRolesAsync(user);
                UserRolesDto usersItems = new(user, getRolById.First());
                return OperationResult<UserRolesDto>.Success(usersItems);
            }
        }
        public async Task<OperationResult<UserQueryFilteredDto>> GetAllAsync(QueryFiltersDto filters)
        {

            var query = _context.Users.AsNoTracking();

            var size = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(filters.Search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(filters.Search.ToLower()) || x.Lastname.ToLower().Contains(filters.Search.ToLower()) || x.NormalizedEmail.Contains(filters.Search.ToUpper()));
            }

            if (filters.Start.HasValue)
            {
                query = query.Skip((int)filters.Start);
            }

            if (filters.Limit.HasValue)
            {
                query = query.Take((int)filters.Limit);
            }


            // var users = await query.ToListAsync();

            var items = await (from r in _applicationContext.UserRoles.AsNoTracking()
                               join us in query on r.UserId equals us.Id
                               join ru in _applicationContext.Roles.AsNoTracking() on r.RoleId equals ru.Id
                               select new UserRolesDto(us, ru.Name)).ToListAsync();

            return OperationResult<UserQueryFilteredDto>.Success(new(size, items));

        }


        public async Task<OperationResult<UserRolesDto>> CreateAsync(CreateUserInput input)
        {
            ApplicationUser user = new();

            user.Id = Guid.NewGuid().ToString();
            user.Name = input.Name;
            user.Lastname = input.Lastname;
            user.UserName = input.Username;
            user.Email = input.Email;
            user.EmailConfirmed = true;


            var result = await _context.CreateAsync(user, input.Password);

            if (!result.Succeeded)
                return OperationResult<UserRolesDto>.Fail(new("error_create_user"));

            var asignRoleToUser = await AsignRoleToUser(user, "User");
            var userById = await _context.Users.SingleAsync(x => x.Id == user.Id);

            result = await _context.AddClaimsAsync(userById, new Claim[]{
                            new Claim(JwtClaimTypes.GivenName, userById.Name),
                            new Claim(JwtClaimTypes.FamilyName, userById.Lastname),
                            new Claim(JwtClaimTypes.Email, userById.Email),
                            new Claim(JwtClaimTypes.Subject, userById.Id)
                        });

            if (!result.Succeeded)
                return OperationResult<UserRolesDto>.Fail(new("error_add_claims_user"));

            UserRolesDto resultView = new(user, "User");
            return OperationResult<UserRolesDto>.Success(resultView);


        }
        public async Task<OperationResult<UserRolesDto>> EditAsync(UserRolesDto user)
        {

            var getUser = _context.FindByIdAsync(user.User.Id).Result;
            getUser.Id = user.User.Id;
            getUser.Email = user.User.Email;
            getUser.UserName = user.User.UserName;

            var response = await _context.UpdateAsync(getUser);

            if (!response.Succeeded)
                return OperationResult<UserRolesDto>.Fail(new("error_edit_user"));

            var asignRoleToUser = await AsignRoleToUser(getUser, user.Role);
            var userById = await _context.Users.SingleAsync(x => x.Id == user.User.Id);

            UserRolesDto resultView = new(userById, user.Role);
            return OperationResult<UserRolesDto>.Success(resultView);

        }
        public async Task<OperationResult> DeleteAsync(string userId)
        {
            var findUserById = await _context.FindByIdAsync(userId);

            if (findUserById is null)
                return OperationResult<UserRolesDto>.NotFound();

            var roleByUser = await _applicationContext.UserRoles.SingleAsync(x => x.UserId == userId);
            _applicationContext.UserRoles.Remove(roleByUser);

            var result = await _context.DeleteAsync(findUserById);

            if (!result.Succeeded)
                return OperationResult<UserRolesDto>.Fail(new("error_delete_user"));

            return OperationResult.Success();

        }

        #endregion

        #region MethodsPrivate
        public async Task<OperationResult> AsignRoleToUser(ApplicationUser user, string role)
        {
            var findUserById = await _context.Users.SingleOrDefaultAsync(x => x.Id == user.Id);

            if (findUserById is null)
                return OperationResult.Fail();

            var rolesByUser = await _context.GetRolesAsync(findUserById);
            if (rolesByUser.Count > 0)
                await _context.RemoveFromRoleAsync(findUserById, rolesByUser.Single());

            var result = await _context.AddToRoleAsync(findUserById, role);

            if (!result.Succeeded)
                return OperationResult.Fail();

            return OperationResult.Success();

        }
        #endregion


    }
}
