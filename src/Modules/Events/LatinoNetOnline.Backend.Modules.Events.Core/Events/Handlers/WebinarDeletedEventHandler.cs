using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Abstractions.Events;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Events.Handlers
{
    internal class WebinarDeletedEventHandler : IEventHandler<WebinarDeletedEventInput>
    {
        private readonly ILogger<WebinarDeletedEventHandler> _logger;
        private readonly IMeetupService _meetupService;
        private readonly IProposalService _proposalService;
        private readonly IWebinarService _webinarService;

        public WebinarDeletedEventHandler(ILogger<WebinarDeletedEventHandler> logger, IMeetupService meetupService, IProposalService proposalService, IWebinarService webinarService)
        {
            _logger = logger;
            _meetupService = meetupService;
            _proposalService = proposalService;
            _webinarService = webinarService;
        }

        public async Task HandleAsync(WebinarDeletedEventInput @event)
        {
            _logger.LogInformation($"Starting WebinarDeletedEventHandler");

            var webinarResult = await _webinarService.GetByIdAsync(new(@event.Id));


            if (!webinarResult.IsSuccess)
            {
                _logger.LogError($"Hubo un error al traer el Webinar.");
                return;
            }


            var eventMeetupResult = await _meetupService.GetMeetupAsync(webinarResult.Result.MeetupId);

            if (!eventMeetupResult.IsSuccess)
            {
                _logger.LogError($"Hubo un error al traer el Meetup.");
                return;
            }

            var deleteEventMeetupResult = await _meetupService.DeleteEventAsync(webinarResult.Result.MeetupId);

            if (!deleteEventMeetupResult.IsSuccess)
            {
                _logger.LogError($"Hubo un error al eliminar el Meetup.");
                return;
            }

            var result = await _webinarService.RemoveAsync(@event.Id);


            if (!result.IsSuccess)
            {
                _logger.LogError($"Hubo un error al remover el Webinar.");
                return;
            }

            result = await _webinarService.UpdateWebinarNumbersAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError($"Hubo un error al actualizar los numeros de Webinars.");
                return;
            }

            _logger.LogInformation($"Finish WebinarDeletedEventHandler");
        }
    }
}
