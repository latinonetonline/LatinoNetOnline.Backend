
using LatinoNetOnline.Backend.Shared.Commons.Enums;

namespace LatinoNetOnline.Backend.Shared.Commons.Paginations
{
    public abstract class QueryStringParameters
    {
        public string SortColumn { get; set; }
        public SortDirection SortDirection { get; set; }
        public int? Start { get; set; }
        public int? Limit { get; set; }
    }
}
