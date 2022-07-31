using LatinoNetOnline.Backend.Modules.Webinars.Core.Services;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Api.Controllers
{
    internal class SpeakersController : BaseController
    {
        private readonly ISpeakerService _service;

        public SpeakersController(ISpeakerService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetAllSpeakers")]
        public async Task<IActionResult> GetAll()
            => new OperationActionResult(await _service.GetAllAsync());

        [HttpGet("Me", Name = "GetSpeaker")]
        [Authorize(Policy = "Anyone")]
        public async Task<IActionResult> Get()
            => new OperationActionResult(await _service.GetAsync());


        [HttpGet("Search", Name = "SearchSpeakers")]
        [Authorize(Policy = "Anyone")]
        public async Task<IActionResult> Search(string search)
            => new OperationActionResult(await _service.SearchAsync(search));
    }
}
