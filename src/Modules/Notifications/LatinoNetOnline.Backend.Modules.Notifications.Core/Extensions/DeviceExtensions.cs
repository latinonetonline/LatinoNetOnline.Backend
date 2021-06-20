using LatinoNetOnline.Backend.Modules.Notifications.Core.Dtos.Devices;
using LatinoNetOnline.Backend.Modules.Notifications.Core.Entities;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Extensions
{
    static class DeviceExtensions
    {
        public static Device ConvertToEntity(this SubscribeDeviceInput input)
            => new(input.PushEndpoint, input.PushP256DH, input.PushAuth, input.UserId, input.UserAgent?.GetOperativeSystem(), input.UserAgent?.GetDeviceName());

        public static DeviceDto ConvertToDto(this Device device)
            => new(device.Id, device.PushEndpoint, device.PushP256DH, device.PushAuth, device.UserId, device.OperativeSystem, device.Name);

        public static Device ModifyEntity(this Device device, SubscribeDeviceInput input)
        {
            device.PushAuth = input.PushAuth;
            device.PushP256DH = input.PushP256DH;
            device.PushEndpoint = input.PushEndpoint;
            device.OperativeSystem = input.UserAgent?.GetOperativeSystem();
            device.Name = input.UserAgent?.GetDeviceName();

            return device;
        }
    }
}
