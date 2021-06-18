using LatinoNetOnline.Backend.Modules.Notifications.Api.Controllers;
using LatinoNetOnline.Backend.Modules.Notifications.Api.Dtos;
using LatinoNetOnline.Backend.Modules.Notifications.Api.Options;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Text.Json;
using System.Threading.Tasks;

using WebPush;

using WebPushDemo.Models;

namespace WebPushDemo.Controllers
{
    class WebPushController : BaseController
    {
        private readonly IConfiguration _configuration;

        private readonly NotificationDbContext _context;
        private readonly VapidKeysOptions _vapidKeysOptions;

        public WebPushController(IConfiguration configuration, NotificationDbContext context, VapidKeysOptions vapidKeysOptions)
        {
            _configuration = configuration;
            _context = context;
            _vapidKeysOptions = vapidKeysOptions;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Send(SendNotification sendNotification)
        {
            var payload = JsonSerializer.Serialize(new
            {
                message = sendNotification.Message,
                url = sendNotification.Url,
            });

            if (sendNotification.DeviceId.HasValue)
            {
                var device = await _context.Devices.SingleOrDefaultAsync(m => m.Id == sendNotification.DeviceId.Value);

                var pushSubscription = new PushSubscription(device.PushEndpoint, device.PushP256DH, device.PushAuth);

                var vapidDetails = new VapidDetails("mailto:example@example.com", _vapidKeysOptions.PublicKey, _vapidKeysOptions.PrivateKey);

                var webPushClient = new WebPushClient();
                webPushClient.SendNotification(pushSubscription, payload, vapidDetails);
            }
            else
            {
                foreach (var device in await _context.Devices.ToListAsync())
                {
                    var pushSubscription = new PushSubscription(device.PushEndpoint, device.PushP256DH, device.PushAuth);

                    var vapidDetails = new VapidDetails("mailto:example@example.com", _vapidKeysOptions.PublicKey, _vapidKeysOptions.PrivateKey);

                    var webPushClient = new WebPushClient();
                    webPushClient.SendNotification(pushSubscription, payload, vapidDetails);
                }
            }



            return Ok(OperationResult.Success());
        }


        [HttpGet]
        public IActionResult GenerateKeys()
        {
            var keys = VapidHelper.GenerateVapidKeys();

            return Ok(keys);
        }
    }
}