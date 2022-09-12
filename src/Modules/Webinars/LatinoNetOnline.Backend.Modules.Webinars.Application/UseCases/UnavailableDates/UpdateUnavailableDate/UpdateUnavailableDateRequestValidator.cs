using FluentValidation;

using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.UpdateUnavailableDate
{
    public class UpdateUnavailableDateRequestValidator : AbstractValidator<UpdateUnavailableDateRequest>
    {
        private readonly IRepository<UnavailableDate> _repository;

        public UpdateUnavailableDateRequestValidator(IRepository<UnavailableDate> repository)
        {
            RuleFor(x => x.Reason).NotEmpty().WithMessage("Ingrese un motivo.");
            RuleFor(x => x.Date).GreaterThanOrEqualTo(DateTime.Today).WithMessage("La fecha debe ser mayor a hoy.");
            RuleFor(x => x.Id).MustAsync(BeExistsAsync).WithMessage("El Id no existe");
            RuleFor(x => x.Date).MustAsync(BeAValidDateAsync).WithMessage("Esa fecha ya esta ocupada. Escoja otra.");
            _repository = repository;
        }

        private async Task<bool> BeAValidDateAsync(UpdateUnavailableDateRequest request, DateTime _, CancellationToken cancellationToken)
        {
            var existDate = await _repository.AnyAsync(x => x.Date == request.Date && x.Id != request.Id, false, cancellationToken);

            return !existDate;
        }

        private async Task<bool> BeExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            var existDate = await _repository.AnyAsync(x => x.Id == id, false, cancellationToken);

            return existDate;
        }
    }
}
