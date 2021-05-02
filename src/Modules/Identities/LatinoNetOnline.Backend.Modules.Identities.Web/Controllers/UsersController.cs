
using IdentityServerHost.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using LatinoNetOnline.Backend.Modules.Identities.Web.Dto;
using LatinoNetOnline.Backend.Modules.Identities.Web.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.OperationResults;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Controllers
{
    class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetFull/{id}")]
        [ProducesResponseType(typeof(OperationResult<UserRolesDto>), 200)]
        public async Task<IActionResult> GetFull(string id)
        {
            return new OperationActionResult(await _userService.GetFullByIdAsync(id));
        }

        [HttpGet("Suggestions")]
        [ProducesResponseType(typeof(OperationResult<UserQueryFilteredDto>), 200)]
        public async Task<IActionResult> GetSuggestions([FromQuery()] QueryFiltersDto filters)
        {
            return new OperationActionResult(await _userService.GetAllAsync(filters));
        }

        [HttpGet("All")]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<ApplicationUser>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return new OperationActionResult(await _userService.GetAllAsync());
        }

        [HttpGet("GetAllRoles")]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<IdentityRole>>), 200)]
        public async Task<IActionResult> GetAllRoles()
        {
            return new OperationActionResult(await _userService.GetAllRolesAsync());
        }

        [HttpPost]
        [ProducesResponseType(typeof(OperationResult<UserRolesDto>), 200)]
        public async Task<IActionResult> CreateAsync(CreateUserInput input)
        {
            return new OperationActionResult(await _userService.CreateAsync(input));
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(typeof(OperationResult), 200)]
        public async Task<IActionResult> DeleteAsync(string userId)
        {
            return new OperationActionResult(await _userService.DeleteAsync(userId));
        }

        [HttpPut("Edit")]
        [ProducesResponseType(typeof(OperationResult<UserRolesDto>), 200)]
        public async Task<IActionResult> EditAsync(UserRolesDto user)
        {
            return new OperationActionResult(await _userService.EditAsync(user));
        }

    }
}
