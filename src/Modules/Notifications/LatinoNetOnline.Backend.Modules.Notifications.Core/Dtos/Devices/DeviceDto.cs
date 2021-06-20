
using System;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Dtos.Devices
{
    record DeviceDto(Guid Id, string PushEndpoint, string PushP256DH, string PushAuth, Guid? UserId, string? OperativeSystem, string? Name);
}
