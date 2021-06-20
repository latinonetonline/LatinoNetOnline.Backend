
using System;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Dtos.Devices
{
    record SubscribeDeviceInput(string PushEndpoint, string PushP256DH, string PushAuth)
    {
        public Guid? UserId { get; set; }
        public string? UserAgent { get; set; }
    }
}
