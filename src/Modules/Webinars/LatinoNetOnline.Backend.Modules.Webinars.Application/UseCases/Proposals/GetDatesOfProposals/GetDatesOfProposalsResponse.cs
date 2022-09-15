using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.GetDatesOfProposals
{
    public record GetDatesOfProposalsResponse(IEnumerable<DateOnly> Dates);
}