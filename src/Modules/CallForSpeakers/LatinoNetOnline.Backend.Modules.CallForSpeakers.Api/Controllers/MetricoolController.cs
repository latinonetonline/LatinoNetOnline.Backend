using LatinoNetOnline.Backend.Modules.Webinars.Core.Services;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Api.Controllers
{
    class MetricoolController : BaseController
    {
        private readonly IMetricoolService _service;

        public MetricoolController(IMetricoolService service)
        {
            _service = service;
        }

        [HttpGet("proposals/{proposalId}")]
        public async Task<IActionResult> ExportFileByWebinar(Guid proposalId)
            => new OperationActionResult(await _service.ExportSocialFileAsync(new(proposalId)));

    }
}
