using FluentValidation;

using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals;

using System;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Validators
{
    class CreateProposalValidator : AbstractValidator<CreateProposalInput>
    {
        public CreateProposalValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Ingrese un título.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Ingrese una descripción de su charla.");

            RuleFor(x => x.Date).GreaterThan(DateTime.Now).WithMessage("La fecha del webinar tiene que ser mayor a hoy.");
        }
    }
}
