using FluentValidation;

using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Dto.Proposals;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Validators
{
    class CreateProposalValidator : AbstractValidator<CreateProposalInput>
    {
        private readonly ApplicationDbContext _dbContext;

        public CreateProposalValidator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Title).NotEmpty().WithMessage("Ingrese un título.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Ingrese una descripción de su charla.");

            RuleFor(x => x.Date).GreaterThan(DateTime.Now).WithMessage("La fecha del webinar tiene que ser mayor a hoy.");

            RuleFor(x => x.Date).Must(BeAValidSaturday).WithMessage("La fecha debe ser un Sábado.");

            RuleFor(x => x.Date).Must(BeAValidDate).WithMessage("Esa fecha ya esta ocupada. Escoja otra.");

        }

        private bool BeAValidSaturday(DateTime date)
        {
            return date.DayOfWeek is DayOfWeek.Saturday;
        }

        private bool BeAValidDate(DateTime date)
        {
            var existDate = _dbContext.Proposals.AsNoTracking().Any(x => x.EventDate.Date == date.Date && x.IsActive);

            return !existDate;
        }
    }
}
