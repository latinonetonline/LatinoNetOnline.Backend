using System;

namespace LatinoNetOnline.Backend.Shared.Abstractions.Entities
{
    public interface IHasCreationTime
    {
        public DateTime CreationTime { get; set; }
    }
}
