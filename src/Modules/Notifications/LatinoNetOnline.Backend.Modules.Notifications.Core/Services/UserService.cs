using LatinoNetOnline.Backend.Modules.Notifications.Core.Dtos.Users;
using LatinoNetOnline.Backend.Shared.Abstractions.Modules;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Services
{
    interface IUserService
    {
        Task<OperationResult<IEnumerable<UserRolesDto>>> GetAllAsync(GetAllUserInput filters);
    }

    class UserService : IUserService
    {
        private readonly IModuleClient _moduleClient;

        public UserService(IModuleClient moduleClient)
        {
            _moduleClient = moduleClient;
        }

        public Task<OperationResult<IEnumerable<UserRolesDto>>> GetAllAsync(GetAllUserInput filters)
            => _moduleClient.GetAsync<OperationResult<IEnumerable<UserRolesDto>>>("modules/users/get", filters);

    }
}
