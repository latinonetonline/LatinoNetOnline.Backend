using LatinoNetOnline.Backend.Modules.Events.Core.Dto.UnavailableDates;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Api.Controllers
{
    internal class UnavailableDatesController : BaseController
    {
        private readonly IUnavailableDateService _service;

        public UnavailableDatesController(IUnavailableDateService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetUnavailableDates")]
        [ProducesResponseType(typeof(OperationResult<UnavailableDateDto[]>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
            => new OperationActionResult(await _service.GetAllAsync());


        [HttpPost(Name = "CreateUnavailableDates")]
        [ProducesResponseType(typeof(OperationResult<UnavailableDateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateUnavailableDateInput input)
            => new OperationActionResult(await _service.CreateAsync(input));


        [HttpPut(Name = "UpdateUnavailableDates")]
        [ProducesResponseType(typeof(OperationResult<UnavailableDateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(UpdateUnavailableDateInput input)
            => new OperationActionResult(await _service.UpdateAsync(input));


        [HttpDelete("{id}", Name = "DeleteUnavailableDates")]
        [ProducesResponseType(typeof(OperationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
            => new OperationActionResult(await _service.DeleteAsync(id));
    }
}
