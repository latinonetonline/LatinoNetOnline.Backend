using FluentValidation;

using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.UnavailableDates;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Validators
{
    internal class CreateUnavailableDateValidator : AbstractValidator<CreateUnavailableDateInput>
    {
        private readonly ApplicationDbContext _dbContext;

        public CreateUnavailableDateValidator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Reason).NotEmpty().WithMessage("Ingrese un motivo.");
            RuleFor(x => x.Date).GreaterThanOrEqualTo(DateTime.Today).WithMessage("La fecha debe ser mayor a hoy.");
            RuleFor(x => x.Date).MustAsync(BeAValidDateAsync).WithMessage("Esa fecha ya esta ocupada. Escoja otra.");

        }

        private async Task<bool> BeAValidDateAsync(DateTime date, CancellationToken cancellationToken)
        {
            var existDate = await _dbContext.UnavailableDates.AsNoTracking().AnyAsync(x => x.Date == date, cancellationToken);

            return !existDate;
        }
    }
}
