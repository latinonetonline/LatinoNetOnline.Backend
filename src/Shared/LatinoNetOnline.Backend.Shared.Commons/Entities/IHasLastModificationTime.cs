using System;

namespace LatinoNetOnline.Backend.Shared.Commons.Entities
{
    public interface IHasLastModificationTime
    {
        public DateTime? LastModificationTime { get; set; }
    }
}
