
using LatinoNetOnline.Backend.Modules.CallForProposals.Api.Requests;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Services;
using LatinoNetOnline.Backend.Modules.Conferences.Api.Controllers;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet("dates")]
        public async Task<IActionResult> GetAllDates()
            => new OperationActionResult(await _proposalService.GetAllDatesAsync());

        [AllowAnonymous]
        [HttpPost]
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
            => new OperationActionResult(await _proposalService.DeleteAsync(id));

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAll()
             => new OperationActionResult(await _proposalService.DeleteAllAsync());

    }
}
