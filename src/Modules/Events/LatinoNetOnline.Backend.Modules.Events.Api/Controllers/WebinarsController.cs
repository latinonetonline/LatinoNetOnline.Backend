using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Api.Controllers
{
    class WebinarsController : BaseController
    {
        private readonly IWebinarService _service;

        public WebinarsController(IWebinarService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet("NextWebinar", Name = "GetNextWebinar")]
        [ProducesResponseType(typeof(OperationResult<WebinarDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNextWebinar()
            => new OperationActionResult(await _service.GetNextWebinarAsync());

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetWebinarById")]
        [ProducesResponseType(typeof(OperationResult<WebinarDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
            => new OperationActionResult(await _service.GetByIdAsync(new(id)));

        [AllowAnonymous]
        [HttpGet("Proposals/{proposalId}", Name = "GetWebinarByProposalId")]
        [ProducesResponseType(typeof(OperationResult<WebinarDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByProposal(Guid proposalId)
            => new OperationActionResult(await _service.GetByProposalAsync(proposalId));

        [AllowAnonymous]
        [HttpGet(Name = "GetWebinars")]
        [ProducesResponseType(typeof(OperationResult<WebinarDto[]>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
            => new OperationActionResult(await _service.GetAllAsync());

        [HttpPut(Name = "UpdateWebinar")]
        [ProducesResponseType(typeof(OperationResult<WebinarDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(UpdateWebinarInput input)
            => new OperationActionResult(await _service.UpdateAsync(input));


        [HttpPost("[action]", Name = "ConfirmWebinar")]
        [ProducesResponseType(typeof(OperationResult<WebinarDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Confirm(ConfirmWebinarInput input)
            => new OperationActionResult(await _service.ConfirmAsync(input));


        [HttpPost("{id}/[action]", Name = "ChangeFlyerWebinar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<WebinarDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Photo(Guid id, IFormFile file)
            => new OperationActionResult(await _service.ChangePhotoAsync(id, file.OpenReadStream()));


        [HttpPut("UpdateNumbers")]
        [ProducesResponseType(typeof(OperationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateNumbers()
            => new OperationActionResult(await _service.UpdateWebinarNumbersAsync());
    }
}
