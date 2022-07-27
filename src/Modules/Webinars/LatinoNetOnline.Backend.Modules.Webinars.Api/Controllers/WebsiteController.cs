using LatinoNetOnline.Backend.Modules.Webinars.Core.Services;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Api.Controllers
{
    class WebsiteController : BaseController
    {
        private readonly IWebsiteService _service;

        public WebsiteController(IWebsiteService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet("PastWebinars")]
        public async Task<IActionResult> GetPastWebinars()
            => new OperationActionResult(await _service.GetPastWebinars());

        [AllowAnonymous]
        [HttpGet("NextWebinar")]
        public async Task<IActionResult> GetNextWebinar()
            => new OperationActionResult(await _service.GetNextWebinars());
    }
}
