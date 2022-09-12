using FluentValidation;

using MediatR;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.DeleteUnavailableDate
{
    public class DeleteUnavailableDateRequestValidator : AbstractValidator<DeleteUnavailableDateRequest>
    {
        public DeleteUnavailableDateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ingrese un nombre.");
        }
    }
}
