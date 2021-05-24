using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Proposals
{
    record ProposalDateDto(IEnumerable<DateTime> Dates);
}
