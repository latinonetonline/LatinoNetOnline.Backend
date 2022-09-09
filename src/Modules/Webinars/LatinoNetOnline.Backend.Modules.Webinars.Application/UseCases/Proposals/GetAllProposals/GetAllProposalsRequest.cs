using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.GetAllProposals
{
    internal record GetAllProposalsRequest(string? Title, DateTime? Date, bool? IsActive, bool? Oldest) : IRequest<OperationResult<IEnumerable<ProposalFullDto>>>
    {
    }
}
