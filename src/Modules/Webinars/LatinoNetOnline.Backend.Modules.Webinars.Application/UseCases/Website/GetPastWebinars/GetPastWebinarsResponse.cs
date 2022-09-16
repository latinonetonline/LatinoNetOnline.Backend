using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Website.GetPastWebinars
{
    public record GetPastWebinarsResponse(IEnumerable<ProposalPublicDto> Proposals);
}