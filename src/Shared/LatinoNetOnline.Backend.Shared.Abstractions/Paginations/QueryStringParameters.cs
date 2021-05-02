
using LatinoNetOnline.Backend.Shared.Abstractions.Enums;

namespace LatinoNetOnline.Backend.Shared.Abstractions.Paginations
{
    public abstract class QueryStringParameters
    {
        public string SortColumn { get; set; }
        public SortDirection SortDirection { get; set; }
        public int? Start { get; set; }
        public int? Limit { get; set; }
    }
}
