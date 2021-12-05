using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Meetups;
using LatinoNetOnline.Backend.Modules.Events.Core.Extensions;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Events.Handlers
{
    class WebinarUpdatedEventHandler : IEventHandler<WebinarUpdatedEventInput>
    {
        private readonly ILogger<WebinarUpdatedEventHandler> _logger;
        private readonly IMeetupService _meetupService;
        private readonly IProposalService _proposalService;
        private readonly IWebinarService _webinarService;

        public WebinarUpdatedEventHandler(ILogger<WebinarUpdatedEventHandler> logger, IMeetupService meetupService, IProposalService proposalService, IWebinarService webinarService)
        {
            _logger = logger;
            _meetupService = meetupService;
            _proposalService = proposalService;
            _webinarService = webinarService;
        }

        public async Task HandleAsync(WebinarUpdatedEventInput @event)
        {
            _logger.LogInformation($"Starting WebinarUpdatedEventHandler");


            var webinarResult = await _webinarService.GetByIdAsync(new(@event.Id));


            if (!webinarResult.IsSuccess)
            {
                _logger.LogError($"Hubo un error al traer el Webinar.");
                return;
            }



            var proposalResult = await _proposalService.GetByIdAsync(new(webinarResult.Result.ProposalId));

            if (!proposalResult.IsSuccess)
            {
                _logger.LogError($"Hubo un error al traer la propuesta.");
                return;
            }

            var eventMeetupResult = await _meetupService.GetMeetupAsync(webinarResult.Result.MeetupId);

            if (!eventMeetupResult.IsSuccess)
            {
                _logger.LogError($"Hubo un error al traer el Meetup.");
                return;
            }

            


            UpdateMeetupEventInput updateMeetupEventInput = new(webinarResult.Result.MeetupId, proposalResult.Result.Proposal.Title, webinarResult.Result.GetDescription(proposalResult.Result), proposalResult.Result.Proposal.EventDate, webinarResult.Result.LiveStreaming, eventMeetupResult.Result?.Photo?.Id);

            eventMeetupResult = await _meetupService.UpdateEventAsync(updateMeetupEventInput);

            if (!eventMeetupResult.IsSuccess || eventMeetupResult.Result is null)
            {
                _logger.LogError($"Hubo un error al modificar el Meetup.");
                return;
            }


            _logger.LogInformation($"Finish WebinarUpdatedEventHandler");
        }
    }
}
