using System;

namespace LatinoNetOnline.Backend.Shared.Commons.Entities
{
    public interface IHasCreationTime
    {
        public DateTime CreationTime { get; set; }
    }
}
