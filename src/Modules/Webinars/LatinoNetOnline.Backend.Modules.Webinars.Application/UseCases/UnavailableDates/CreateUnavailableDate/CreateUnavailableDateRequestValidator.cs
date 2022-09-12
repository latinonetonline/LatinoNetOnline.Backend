using FluentValidation;

using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.GenericRepository.Repositories;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.CreateUnavailableDate
{
    public class CreateUnavailableDateRequestValidator : AbstractValidator<CreateUnavailableDateRequest>
    {
        private readonly IRepository<UnavailableDate> _repository;

        public CreateUnavailableDateRequestValidator(IRepository<UnavailableDate> repository)
        {
            RuleFor(x => x.Reason).NotEmpty().WithMessage("Ingrese un motivo.");
            RuleFor(x => x.Date).GreaterThanOrEqualTo(DateTime.Today).WithMessage("La fecha debe ser mayor a hoy.");
            RuleFor(x => x.Date).MustAsync(BeAValidDateAsync).WithMessage("Esa fecha ya esta ocupada. Escoja otra.");
            _repository = repository;
        }

        private async Task<bool> BeAValidDateAsync(DateTime date, CancellationToken cancellationToken)
        {
            var existDate = await _repository.AnyAsync(x => x.Date == date, false, cancellationToken);

            return !existDate;
        }
    }
}
