using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.UnavailableDates;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Api.Controllers
{
    internal class UnavailableDatesController : BaseController
    {
        private readonly IUnavailableDateService _service;

        public UnavailableDatesController(IUnavailableDateService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => new OperationActionResult(await _service.GetAllAsync());


        [HttpPost]
        public async Task<IActionResult> Create(CreateUnavailableDateInput input)
            => new OperationActionResult(await _service.CreateAsync(input));


        [HttpPut]
        public async Task<IActionResult> Update(UpdateUnavailableDateInput input)
            => new OperationActionResult(await _service.UpdateAsync(input));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
            => new OperationActionResult(await _service.DeleteAsync(id));
    }
}
