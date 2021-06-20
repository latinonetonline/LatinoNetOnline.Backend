
using System;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Dtos.Devices
{
    class DeviceFilter
    {
        public Guid? UserId { get; set; }
        public string? Name { get; set; }
        public string? OperativeSystem { get; set; }
    }
}
