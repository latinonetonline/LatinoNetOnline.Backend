using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Api.Controllers
{
    class MeetupController : BaseController
    {
        private readonly IMeetupService _service;

        public MeetupController(IMeetupService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetEvents()
            => new OperationActionResult(await _service.GetEventsAsync());

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
            => new OperationActionResult(await _service.GetMeetupAsync(id));
    }
}
