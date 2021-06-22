
using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Dtos.Devices
{
    class DeviceFilter
    {
        public IEnumerable<Guid>? Users { get; set; }
        public string? Name { get; set; }
        public string? OperativeSystem { get; set; }
    }
}
