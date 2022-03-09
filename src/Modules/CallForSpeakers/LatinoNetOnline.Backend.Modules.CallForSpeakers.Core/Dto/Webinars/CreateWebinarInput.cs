
using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars
{
    record CreateWebinarInput(Guid ProposalId, DateTime StartDateTime)
    {
        public int Number { get; set; }
    }
}