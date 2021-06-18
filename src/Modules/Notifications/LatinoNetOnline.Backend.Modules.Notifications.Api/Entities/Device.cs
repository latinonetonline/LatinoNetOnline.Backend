using System;

namespace WebPushDemo.Models
{
    public class Device
    {
        public Guid Id { get; set; }
        public string PushEndpoint { get; set; }
        public string PushP256DH { get; set; }
        public string PushAuth { get; set; }
        public Guid? UserId { get; set; }
    }
}
