using System;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals
{
    class ProposalFilter
    {
        public string Title { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsActive { get; set; }
    }
}
