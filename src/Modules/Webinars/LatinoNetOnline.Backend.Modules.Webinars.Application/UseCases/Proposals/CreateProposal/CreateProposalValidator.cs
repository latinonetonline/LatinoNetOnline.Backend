using FluentValidation;

using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Validators;
using LatinoNetOnline.GenericRepository.Repositories;

using Microsoft.EntityFrameworkCore;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.CreateProposal
{
    class CreateProposalValidator : AbstractValidator<CreateProposalRequest>
    {
        private readonly IRepository<Proposal> _dbContext;

        public CreateProposalValidator(IRepository<Proposal> dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Title).NotEmpty().WithMessage("Ingrese un título.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Ingrese una descripción de su charla.");

            RuleFor(x => x.Date).GreaterThan(DateTime.Now).WithMessage("La fecha del webinar tiene que ser mayor a hoy.");

            RuleFor(x => x.Date).Must(BeAValidSaturday).WithMessage("La fecha debe ser un Sábado.");

            RuleFor(x => x.Date).MustAsync(BeAValidDateAsync).WithMessage("Esa fecha ya esta ocupada. Escoja otra.");

            RuleFor(x => x.Speakers).NotEmpty().WithMessage("Debe especificar minimo un Spekaer.");

            RuleForEach(x => x.Speakers).SetValidator(new CreateSpeakerValidator());

        }

        private bool BeAValidSaturday(DateTime date)
        {
            return date.DayOfWeek is DayOfWeek.Saturday;
        }

        private async Task<bool> BeAValidDateAsync(DateTime date, CancellationToken cancellationToken)
        {
            var existDate = await _dbContext.AnyAsync(x => x.EventDate.Date == date.Date && x.IsActive, false, cancellationToken);

            return !existDate;
        }
    }
}
