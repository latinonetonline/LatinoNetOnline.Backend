using LatinoNetOnline.Backend.Modules.Events.Api.Controllers;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Api.Controllers
{
    class MetricoolController : BaseController
    {
        private readonly IMetricoolService _service;

        public MetricoolController(IMetricoolService service)
        {
            _service = service;
        }

        [HttpGet("webinars/{webinarId}")]
        public async Task<IActionResult> ExportFileByWebinar(Guid webinarId)
            => new OperationActionResult(await _service.ExportSocialFileAsync(new(webinarId)));

    }
}
