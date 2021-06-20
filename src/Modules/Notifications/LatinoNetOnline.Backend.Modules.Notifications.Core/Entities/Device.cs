
using System;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Entities
{
    class Device
    {
        public Device()
        {
        }

        public Device(string pushEndpoint, string pushP256DH, string pushAuth, Guid? userId, string? operativeSystem, string? deviceName)
        {
            PushEndpoint = pushEndpoint;
            PushP256DH = pushP256DH;
            PushAuth = pushAuth;
            UserId = userId;
            OperativeSystem = operativeSystem;
            Name = deviceName;
        }

        public Guid Id { get; set; }

        private string? _pushEndpoint;

        public string PushEndpoint
        {
            set => _pushEndpoint = value;
            get => _pushEndpoint
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(PushEndpoint));
        }


        private string? _pushP256DH;

        public string PushP256DH
        {
            set => _pushP256DH = value;
            get => _pushP256DH
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(PushP256DH));
        }



        private string? _pushAuth;

        public string PushAuth
        {
            set => _pushAuth = value;
            get => _pushAuth
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(PushAuth));
        }

        public Guid? UserId { get; set; }
        public string? OperativeSystem { get; set; }
        public string? Name { get; set; }
    }
}
