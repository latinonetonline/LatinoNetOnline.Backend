using FluentValidation;

using MediatR;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.GetDatesOfProposals
{
    public class GetDatesOfProposalsRequestValidator : AbstractValidator<GetDatesOfProposalsRequest>
    {
        public GetDatesOfProposalsRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ingrese un nombre.");
        }
    }
}
