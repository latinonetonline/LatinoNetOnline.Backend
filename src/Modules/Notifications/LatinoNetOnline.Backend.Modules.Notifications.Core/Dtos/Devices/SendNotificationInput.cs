
using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Dtos.Devices
{
    record SendNotificationInput(IEnumerable<Guid> Devices, string Message, Uri Url);
}
