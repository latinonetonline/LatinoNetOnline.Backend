
using CSharpFunctionalExtensions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using LatinoNetOnline.Backend.Modules.CallForProposals.Api.Requests;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Services;
using LatinoNetOnline.Backend.Modules.Conferences.Api.Controllers;
using LatinoNetOnline.Backend.Shared.Abstractions.Extensions;
using LatinoNetOnline.Backend.Shared.Abstractions.OperationResults;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatinoNETOnline.App.Api.Controllers
{
    class ProposalsController : BaseController
    {
        private readonly IProposalService _proposalService;
        private readonly ISpeakerService _speakerService;


        public ProposalsController(IProposalService proposalService, ISpeakerService speakerService)
        {
            _proposalService = proposalService;
            _speakerService = speakerService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await _proposalService.GetAllAsync()
                .Finally(result => new OperationActionResult(result.IsSuccess ?OperationResult<IEnumerable<ProposalFullDto>>.Success(result.Value) :
                  OperationResult<IEnumerable<ProposalFullDto>>.Fail(new(result.Error))));

        }

        [AllowAnonymous]
        [HttpGet("dates")]
        public async Task<IActionResult> GetAllDates()
        {
            return await _proposalService.GetAllDatesAsync()
                .Finally(result => new OperationActionResult(result.IsSuccess ? OperationResult<ProposalDateDto>.Success(result.Value) :
                  OperationResult<ProposalDateDto>.Fail(new(result.Error))));

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file, [FromForm] CreateProposalRequest request)
        {
            return await _speakerService.CreateAsync(new(request.Name, request.LastName, request.Email, request.Twitter, request.SpeakerDescription, file.OpenReadStream().ReadFully()))
                .Bind(speaker => _proposalService.CreateAsync(new(speaker.SpeakerId, request.ProposalTitle, request.ProposalDescription, request.Date, request.AudienceAnswer, request.KnowledgeAnswer, request.UseCaseAnswer)))
                .Bind(detail => _proposalService.GetByIdAsync(detail.ProposalId))
                .Finally(result => new OperationActionResult(result.IsSuccess ? OperationResult<ProposalFullDto>.Success(result.Value) :
                OperationResult<ProposalFullDto>.Fail(new(result.Error))));
        }
    }
}
