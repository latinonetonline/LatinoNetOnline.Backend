﻿using LatinoNetOnline.Backend.Modules.CallForSpeakers.Api.Controllers;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Api.Controllers
{
    class MeetupController : BaseController
    {
        private readonly IMeetupService _service;

        public MeetupController(IMeetupService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
            => new OperationActionResult(await _service.GetMeetupAsync(id));
    }
}
