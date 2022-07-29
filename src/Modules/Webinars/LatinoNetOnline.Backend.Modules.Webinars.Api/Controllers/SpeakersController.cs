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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
            => new OperationActionResult(await _service.GetAllAsync());

        [HttpGet("Me")]
        public async Task<IActionResult> Get()
            => new OperationActionResult(await _service.GetAsync());


        [HttpGet("Search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string search)
            => new OperationActionResult(await _service.SearchAsync(search));
    }
}
