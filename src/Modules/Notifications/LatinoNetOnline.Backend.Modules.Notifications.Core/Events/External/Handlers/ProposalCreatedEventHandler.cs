using LatinoNetOnline.Backend.Shared.Abstractions.Events;
using LatinoNetOnline.Backend.Shared.Abstractions.Modules;

using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Events.External.Handlers
{
    class ProposalCreatedEventHandler : IEventHandler<ProposalCreatedEventInput>
    {
        private readonly ILogger<ProposalCreatedEventHandler> _logger;
        private readonly IModuleClient _moduleClient;


        public ProposalCreatedEventHandler(ILogger<ProposalCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(ProposalCreatedEventInput eventInput)
        {
            // long-time running process
            await Task.Delay(10_000);
            _logger.LogInformation($"Received event about created conference with ID: {eventInput.Id}");
        }
    }
}
