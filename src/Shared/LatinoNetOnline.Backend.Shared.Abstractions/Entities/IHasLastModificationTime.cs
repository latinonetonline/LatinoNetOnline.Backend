using System;

namespace LatinoNetOnline.Backend.Shared.Abstractions.Entities
{
    public interface IHasLastModificationTime
    {
        public DateTime? LastModificationTime { get; set; }
    }
}
