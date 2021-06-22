using LatinoNetOnline.Backend.Modules.Notifications.Core.Dtos.Devices;
using LatinoNetOnline.Backend.Modules.Notifications.Core.Services;
using LatinoNetOnline.Backend.Shared.Infrastructure.Presenter;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

using System;
using System.Threading.Tasks;

using WebPush;

namespace LatinoNetOnline.Backend.Modules.Notifications.Api.Controllers
{
    class DevicesController : BaseController
    {
        private readonly IDeviceService _service;

        public DevicesController(IDeviceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DeviceFilter filter)
            => new OperationActionResult(await _service.GetAllAsync(filter));

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Subscribe(SubscribeDeviceInput input)
        {
            input.UserAgent = Request.Headers[HeaderNames.UserAgent].ToString();

            if (HttpContext.User is not null && HttpContext.User.Identity.IsAuthenticated)
            {
                Guid? userId = Guid.Parse(HttpContext.User.FindFirst("sub").Value);
                input.UserId = userId;
            }

            return new OperationActionResult(await _service.SubscribeAsync(input));
        }


        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult GetVapidPublicKey()
            => new OperationActionResult(_service.GetVapidPublicKey());

        [HttpPost("[action]")]
        public async Task<IActionResult> SendNotification(SendNotificationInput input)
            => new OperationActionResult(await _service.SendNotificationAsync(input));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
             => new OperationActionResult(await _service.DeleteAsync(id));



        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult GenerateVapidKeys()
        {
            var keys = VapidHelper.GenerateVapidKeys();

            return Ok(keys);
        }
    }
}
