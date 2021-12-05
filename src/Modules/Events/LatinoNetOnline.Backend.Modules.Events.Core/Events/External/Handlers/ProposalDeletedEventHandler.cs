using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Events.External.Handlers
{
    class ProposalDeletedEventHandler : IEventHandler<ProposalDeletedEventInput>
    {
        private readonly ILogger<ProposalDeletedEventHandler> _logger;
        private readonly IProposalService _proposalService;
        private readonly IMeetupService _meetupService;
        private readonly IWebinarService _webinarService;

        public ProposalDeletedEventHandler(ILogger<ProposalDeletedEventHandler> logger, IProposalService proposalService, IMeetupService meetupService, IWebinarService webinarService)
        {
            _logger = logger;
            _proposalService = proposalService;
            _meetupService = meetupService;
            _webinarService = webinarService;
        }

        public async Task HandleAsync(ProposalDeletedEventInput @event)
        {
            _logger.LogInformation($"Starting ProposalDeletedEventHandler");


            var webinarResult = await _webinarService.GetByProposalAsync(@event.Id);


            if (!webinarResult.IsSuccess)
            {
                _logger.LogError($"Hubo un error al traer el Webinar.");
                return;
            }


            var deleteWebinarResult = await _webinarService.DeleteAsync(webinarResult.Result.Id);

            if (!deleteWebinarResult.IsSuccess)
            {
                _logger.LogError($"Hubo un error al eliminar el Webinar.");
                return;
            }

            _logger.LogInformation($"Finish ProposalDeletedEventHandler");
        }
    }
}
