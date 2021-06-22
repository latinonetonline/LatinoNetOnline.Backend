using LatinoNetOnline.Backend.Modules.Notifications.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;
using LatinoNetOnline.Backend.Shared.Abstractions.Modules;

using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Events.External.Handlers
{
    class ProposalCreatedEventHandler : IEventHandler<ProposalCreatedEventInput>
    {
        private readonly ILogger<ProposalCreatedEventHandler> _logger;
        private readonly IUserService _userService;
        private readonly IDeviceService _deviceService;

        public ProposalCreatedEventHandler(ILogger<ProposalCreatedEventHandler> logger, IUserService userService, IDeviceService deviceService)
        {
            _logger = logger;
            _userService = userService;
            _deviceService = deviceService;
        }

        public async Task HandleAsync(ProposalCreatedEventInput eventInput)
        {

            _logger.LogInformation($"Starting ProposalCreatedEventHandler");

            var adminUsersResult = await _userService.GetAllAsync(new Dtos.Users.GetAllUserInput
            {
                Role = "Admin"
            });

            if (!adminUsersResult.IsSuccess)
            {
                _logger.LogError($"Hubo un error al traer a los usuarios administradores.");
                return;
            }

            var devicesResult = await _deviceService.GetAllAsync(new Dtos.Devices.DeviceFilter
            {
                Users = adminUsersResult.Result.Select(x => x.Id)
            });

            if (!devicesResult.IsSuccess)
            {
                _logger.LogError($"Hubo un error al traer a los dispositivos.");
                return;
            }

            var result = await _deviceService.SendNotificationAsync(new(devicesResult.Result.Select(x => x.Id), $"Call For Speakers: Propusieron la charla '{eventInput.Title}'", new("https://admin.latinonet.online/CallForSpeakers/" + eventInput.Id)));

            if (!result.IsSuccess)
            {
                _logger.LogError($"Hubo un error al enviar las notificaciones.");
                return;
            }

            _logger.LogInformation($"Finish ProposalCreatedEventHandler");
        }
    }
}
