using LatinoNetOnline.Backend.Modules.Notifications.Core.Data;
using LatinoNetOnline.Backend.Modules.Notifications.Core.Dtos.Devices;
using LatinoNetOnline.Backend.Modules.Notifications.Core.Entities;
using LatinoNetOnline.Backend.Modules.Notifications.Core.Extensions;
using LatinoNetOnline.Backend.Modules.Notifications.Core.Options;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using WebPush;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Services
{
    interface IDeviceService
    {
        Task<OperationResult> DeleteAsync(Guid id);
        Task<OperationResult<IEnumerable<DeviceDto>>> GetAllAsync(DeviceFilter filter);
        OperationResult<VapidPublicKey> GetVapidPublicKey();
        Task<OperationResult<DeviceDto>> SubscribeAsync(SubscribeDeviceInput input);
        Task<OperationResult> SendNotificationAsync(SendNotificationInput input);
    }

    class DeviceService : IDeviceService
    {
        private readonly NotificationDbContext _dbContext;
        private readonly VapidKeysOptions _vapidKeysOptions;

        public DeviceService(NotificationDbContext dbContext, VapidKeysOptions vapidKeysOptions)
        {
            _dbContext = dbContext;
            _vapidKeysOptions = vapidKeysOptions;
        }

        public async Task<OperationResult<IEnumerable<DeviceDto>>> GetAllAsync(DeviceFilter filter)
        {
            var devices = await _dbContext.Devices
                .WhereIf(filter.Users?.Any() ?? false, x => filter.Users.Contains(x.UserId ?? default))
                .WhereIf(filter.OperativeSystem is not null, x => x.OperativeSystem == filter.OperativeSystem)
                .WhereIf(filter.Name is not null, x => x.Name == filter.Name)
                .Select(x => x.ConvertToDto())
                .ToListAsync();


            return OperationResult<IEnumerable<DeviceDto>>.Success(devices);
        }

        public async Task<OperationResult<DeviceDto>> SubscribeAsync(SubscribeDeviceInput input)
        {
            Device? device = await _dbContext.Devices.SingleOrDefaultAsync(x => x.PushEndpoint == input.PushEndpoint && x.UserId == input.UserId);


            if (device is null)
            {
                device = input.ConvertToEntity();
                await _dbContext.AddAsync(device);
            }
            else
            {
                device.ModifyEntity(input);
                _dbContext.Update(device);
            }

            await _dbContext.SaveChangesAsync();

            return OperationResult<DeviceDto>.Success(device.ConvertToDto());
        }

        public async Task<OperationResult> SendNotificationAsync(SendNotificationInput input)
        {
            var payload = JsonSerializer.Serialize(new
            {
                message = input.Message,
                url = input.Url,
            });

            if (input.Devices.Any())
            {
                var devices = await _dbContext.Devices.Where(x => input.Devices.Contains(x.Id)).ToListAsync();

                await SendNotification(devices);
            }
            else
            {

                await SendNotification(await _dbContext.Devices.ToListAsync());
            }


            async Task SendNotification(IEnumerable<Device> devices)
            {
                foreach (var device in devices)
                {
                    var pushSubscription = new PushSubscription(device.PushEndpoint, device.PushP256DH, device.PushAuth);

                    var vapidDetails = new VapidDetails("mailto:example@example.com", _vapidKeysOptions.PublicKey, _vapidKeysOptions.PrivateKey);

                    var webPushClient = new WebPushClient();

                    try
                    {
                        await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
                    }
                    catch (Exception)
                    {
                    }
                }
            }


            return OperationResult.Success();
        }



        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            Device? device = await _dbContext.Devices.SingleOrDefaultAsync(x => x.Id == id);

            if (device is null)
                return OperationResult.Fail();

            _dbContext.Devices.Remove(device);
            await _dbContext.SaveChangesAsync();

            return OperationResult.Success();
        }

        public OperationResult<VapidPublicKey> GetVapidPublicKey()
            => OperationResult<VapidPublicKey>.Success(new(_vapidKeysOptions.PublicKey));

    }
}
