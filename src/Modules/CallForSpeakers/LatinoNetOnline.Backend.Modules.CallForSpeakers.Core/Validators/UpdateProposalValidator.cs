using FluentValidation;

using LatinoNetOnline.Backend.Modules.Webinars.Core.Data;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Proposals;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Validators
{
    class UpdateProposalValidator : AbstractValidator<UpdateProposalInput>
    {
        private readonly ApplicationDbContext _dbContext;

        public UpdateProposalValidator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Title).NotEmpty().WithMessage("Ingrese un título.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Ingrese una descripción de su charla.");

            RuleFor(x => x.Date).Must(BeAValidSaturday).WithMessage("La fecha debe ser un Sábado.");

            RuleFor(x => x.Date).MustAsync(BeAValidDateAsync).WithMessage("Esa fecha ya esta ocupada. Escoja otra.");

            RuleFor(x => x.Speakers).NotEmpty().WithMessage("Debe especificar minimo un Spekaer.");

            RuleForEach(x => x.Speakers).SetValidator(new UpdateSpeakerValidator());

        }
        private bool BeAValidSaturday(DateTime date)
        {
            return date.DayOfWeek is DayOfWeek.Saturday;
        }

        private async Task<bool> BeAValidDateAsync(UpdateProposalInput input, DateTime date, CancellationToken cancellationToken)
        {
            var existDate = await _dbContext.Proposals.AsNoTracking().AnyAsync(x => x.EventDate.Date == date.Date && x.Id != input.Id && x.IsActive, cancellationToken);

            return !existDate;
        }
    }
}
