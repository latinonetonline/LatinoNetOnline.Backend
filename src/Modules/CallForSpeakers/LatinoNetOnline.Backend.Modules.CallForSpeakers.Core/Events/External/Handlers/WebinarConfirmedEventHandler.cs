using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Extensions;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Managers;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Events.External.Handlers
{
    class WebinarConfirmedEventHandler : IEventHandler<WebinarConfirmedEventInput>
    {
        private readonly ILogger<WebinarConfirmedEventHandler> _logger;
        private readonly IProposalService _proposalService;
        private readonly IWebinarService _webinarService;
        private readonly IEmailManager _emailManager;

        public WebinarConfirmedEventHandler(ILogger<WebinarConfirmedEventHandler> logger, IProposalService proposalService, IWebinarService webinarService, IEmailManager emailManager)
        {
            _logger = logger;
            _proposalService = proposalService;
            _webinarService = webinarService;
            _emailManager = emailManager;
        }

        public async Task HandleAsync(WebinarConfirmedEventInput @event)
        {
            var webinarResult = await _webinarService.GetAsync(new(@event.Id));

            if (!webinarResult.IsSuccess)
            {
                _logger.LogError("Hubo un error al traer el webinar.");
                return;
            }

            var proposalResult = await _proposalService.GetByIdAsync(new(webinarResult.Result.ProposalId));

            if (!proposalResult.IsSuccess)
            {
                _logger.LogError("Hubo un error al traer la propuesta.");
                return;
            }

            var emailInput = await proposalResult.Result.ConvertToProposalConfirmedEmailInput(webinarResult.Result);

            var emailResult = await _emailManager.SendEmailAsync(emailInput);

            if (!emailResult.IsSuccess)
            {
                _logger.LogError("Hubo un error al enviar el Email.");
                return;
            }
        }
    }
}
