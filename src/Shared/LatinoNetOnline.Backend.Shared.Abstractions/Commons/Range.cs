using System;

namespace LatinoNetOnline.Backend.Shared.Abstractions.Commons
{
    public record Range<T>(T Min, T Max) where T : IComparable
    {

        public bool IsOverlapped(Range<T> other) => Min.CompareTo(other.Max) < 0 && other.Min.CompareTo(Max) < 0;


        public bool IsInside(Range<T> other) => Min.CompareTo(other.Min) <= 0 && Max.CompareTo(other.Max) >= 0;

    }
}
