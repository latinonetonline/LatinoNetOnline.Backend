﻿using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("nextevent")]
        public async Task<IActionResult> GetNextWebinar()
            => new OperationActionResult(await _service.GetNextWebinarAsync());

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
            => new OperationActionResult(await _service.GetByIdAsync(id));

        [AllowAnonymous]
        [HttpGet("Proposals/{proposalId}")]
        public async Task<IActionResult> GetByProposal(Guid proposalId)
            => new OperationActionResult(await _service.GetByProposalAsync(proposalId));

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => new OperationActionResult(await _service.GetAllAsync());


        [HttpPost]
        public async Task<IActionResult> Create(CreateWebinarInput input)
            => new OperationActionResult(await _service.CreateAsync(input));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
            => new OperationActionResult(await _service.DeleteAsync(id));
    }
}