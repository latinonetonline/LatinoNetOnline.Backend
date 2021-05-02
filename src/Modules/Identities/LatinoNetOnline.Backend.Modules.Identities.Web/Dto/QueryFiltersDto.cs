
using System;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Dto
{
    class QueryFiltersDto
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? Start { get; set; }
        public int? Limit { get; set; }
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }
        public string? Search { get; set; }
    }
}
