using LatinoNetOnline.Backend.Shared.Commons.Entities;

using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Entities
{
    public class UnavailableDate : IEntity
    {
        public UnavailableDate(DateTime date, string reason)
        {
            Date = date;
            Reason = reason;
        }

        public UnavailableDate()
        {
        }

        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        private string? _reason;

        public string Reason
        {
            set => _reason = value;
            get => _reason
                   ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Reason));
        }
    }
}
