
using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Dtos
{
    record SendNotificationInput(IEnumerable<Guid> Devices, string Message, Uri Url);
}
