using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Events.External.Handlers
{
    class ProposalCreatedEventHandler : IEventHandler<ProposalCreatedEventInput>
    {
        private readonly ILogger<ProposalCreatedEventHandler> _logger;
        private readonly IProposalService _proposalService;
        private readonly IWebinarService _webinarService;

        public ProposalCreatedEventHandler(ILogger<ProposalCreatedEventHandler> logger, IProposalService proposalService, IWebinarService webinarService)
        {
            _logger = logger;
            _proposalService = proposalService;
            _webinarService = webinarService;
        }

        public async Task HandleAsync(ProposalCreatedEventInput eventInput)
        {

            _logger.LogInformation($"Starting ProposalCreatedEventHandler");

            var proposalResult = await _proposalService.GetByIdAsync(new(eventInput.Id));

            if (!proposalResult.IsSuccess)
            {
                _logger.LogError($"Hubo un error al traer la propuesta.");
                return;
            }

            var webinarResult = await _webinarService.CreateAsync(new(proposalResult.Result.Proposal.ProposalId, proposalResult.Result.Proposal.EventDate));

            if (!webinarResult.IsSuccess)
            {
                _logger.LogError($"Hubo un error al crear el Webinar.");
                return;
            }

            _logger.LogInformation($"Finish ProposalCreatedEventHandler");
        }
    }
}
