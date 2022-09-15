using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.GetProposalById
{
    public record GetProposalByIdResponse(ProposalDto Proposal, IEnumerable<SpeakerDto> Speakers);
}