using FluentValidation;

using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.ProposalSpeakers;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Validators
{
    class CreateProposalSpeakerValidator : AbstractValidator<CreateProposalSpeakerInput>
    {
        public CreateProposalSpeakerValidator()
        {
        }
    }
}
