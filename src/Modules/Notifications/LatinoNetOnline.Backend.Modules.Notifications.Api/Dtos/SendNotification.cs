using System;

namespace LatinoNetOnline.Backend.Modules.Notifications.Api.Dtos
{
    class SendNotification
    {
        public Guid? DeviceId { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}
