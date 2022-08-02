
using LatinoNetOnline.Backend.Modules.Webinars.Api.Controllers;
using LatinoNetOnline.Backend.Modules.Webinars.Api.Requests;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.Extensions;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace LatinoNETOnline.App.Api.Controllers
{
    class ProposalsController : BaseController
    {
        private readonly IProposalService _proposalService;

        public ProposalsController(IProposalService proposalService)
        {
            _proposalService = proposalService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProposalFilter filter)
            => new OperationActionResult(await _proposalService.GetAllAsync(filter));

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
            => new OperationActionResult(await _proposalService.GetByIdAsync(new(id)));

        [AllowAnonymous]
        [HttpGet("dates", Name = "GetDates")]
        [ProducesResponseType(typeof(OperationResult<ProposalDateDto>), 200)]
        public async Task<IActionResult> GetAllDates()
            => new OperationActionResult(await _proposalService.GetDatesAsync());

        [HttpPost(Name = "CreateProposal")]
        [Authorize(Policy = "Anyone")]
        [ProducesResponseType(typeof(OperationResult<ProposalFullDto>), 200)]
        public async Task<IActionResult> Create(CreateProposalRequest request)
        {
            var result = await _proposalService.CreateAsync(new(
                request.Title,
                request.Description,
                request.Date,
                request.AudienceAnswer,
                request.KnowledgeAnswer,
                request.UseCaseAnswer,
                request.Speakers));

            return new OperationActionResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProposalInput input)
            => new OperationActionResult(await _proposalService.UpdateAsync(input));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
            => new OperationActionResult(await _proposalService.DeleteAsync(id));

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAll()
             => new OperationActionResult(await _proposalService.DeleteAllAsync());

        [HttpPost("{id}/[action]")]
        public async Task<IActionResult> Photo(Guid id, IFormFile file)
            => new OperationActionResult(await _proposalService.ChangePhotoAsync(id, file.OpenReadStream().ReadFully()));

        [HttpPost("[action]")]
        public async Task<IActionResult> Confirm(ConfirmProposalInput input)
            => new OperationActionResult(await _proposalService.ConfirmProposalAsync(input));

        [HttpPut("UpdateNumbers")]
        public async Task<IActionResult> UpdateNumbers()
            => new OperationActionResult(await _proposalService.UpdateWebinarNumbersAsync());

        [HttpGet("{id}/Description")]
        public async Task<IActionResult> GetDescription(Guid id)
            => new OperationActionResult(await _proposalService.GetDescriptionTextAsync(new(id)));

        [HttpPut("UpdateViews")]
        public async Task<IActionResult> UpdateViews(UpdateProposalViewsInput input)
            => new OperationActionResult(await _proposalService.UpdateViewsAsync(input));

    }
}
