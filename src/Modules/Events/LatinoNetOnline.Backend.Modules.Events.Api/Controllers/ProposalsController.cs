using LatinoNetOnline.Backend.Modules.Events.Api.Requests;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Proposals;
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
    class ProposalsController : BaseController
    {
        private readonly IProposalService _proposalService;

        public ProposalsController(IProposalService proposalService)
        {
            _proposalService = proposalService;
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetProposals")]
        [ProducesResponseType(typeof(OperationResult<ProposalFullDto[]>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] ProposalFilter filter)
            => new OperationActionResult(await _proposalService.GetAllAsync(filter));

        [HttpGet("{id}", Name = "GetProposalById")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(OperationResult<ProposalFullDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
            => new OperationActionResult(await _proposalService.GetByIdAsync(new(id)));

        [AllowAnonymous]
        [HttpGet("dates", Name = "GetProposalDates")]
        [ProducesResponseType(typeof(OperationResult<ProposalDateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDates()
            => new OperationActionResult(await _proposalService.GetDatesAsync());

        [AllowAnonymous]
        [HttpPost(Name = "CreateProposal")]
        [ProducesResponseType(typeof(OperationResult<ProposalFullDto>), StatusCodes.Status200OK)]
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

        [HttpPut(Name = "UpdateProposal")]
        [ProducesResponseType(typeof(OperationResult<ProposalFullDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(UpdateProposalInput input)
            => new OperationActionResult(await _proposalService.UpdateAsync(input));


        [HttpDelete("{id}", Name = "DeleteProposal")]
        [ProducesResponseType(typeof(OperationResult<ProposalFullDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
            => new OperationActionResult(await _proposalService.DeleteAsync(id));

    }
}
