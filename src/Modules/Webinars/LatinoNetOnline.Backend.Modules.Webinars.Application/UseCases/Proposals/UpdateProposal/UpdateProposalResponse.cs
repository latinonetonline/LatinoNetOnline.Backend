using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.UpdateProposal
{
    public record UpdateProposalResponse(ProposalDto Proposal, IEnumerable<SpeakerDto> Speakers);
}