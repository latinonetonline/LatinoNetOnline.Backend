using System;
using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals
{
    record ProposalViewsDto(Guid Id, int? Views);
    record UpdateProposalViewsInput(IEnumerable<ProposalViewsDto> Views);
}
