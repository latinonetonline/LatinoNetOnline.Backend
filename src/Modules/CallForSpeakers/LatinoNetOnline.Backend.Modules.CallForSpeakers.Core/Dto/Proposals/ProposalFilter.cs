using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Proposals
{
    record ProposalFilter
    {
        public string? Title { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsActive { get; set; }
    }
}
