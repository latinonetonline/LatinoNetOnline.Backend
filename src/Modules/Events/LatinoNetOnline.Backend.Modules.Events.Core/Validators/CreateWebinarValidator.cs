using FluentValidation;

using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Validators
{
    class CreateWebinarValidator : AbstractValidator<CreateWebinarInput>
    {
        private readonly IProposalService _proposalService;
        private readonly IMeetupService _meetupService;

        public CreateWebinarValidator(IProposalService proposalService, IMeetupService meetupService)
        {
            _proposalService = proposalService;
            _meetupService = meetupService;

            RuleFor(x => x.MeetupId).GreaterThan(0).WithMessage("Ingrese un Id de Meetup valido.");
            RuleFor(x => x.MeetupId).MustAsync(BeValidMeetupAsync).WithMessage("No existe un Meetup con ese Id.");

            RuleFor(x => x.ProposalId).MustAsync(BeExistProposalAsync).WithMessage("No existe una propuesta con ese Id.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Ingrese un Titulo valido.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Ingrese una Descripción valida.");

        }

        private async Task<bool> BeExistProposalAsync(Guid proposalId, CancellationToken cancellationToken)
        {
            var result = await _proposalService.GetByIdAsync(new(proposalId));

            return result.IsSuccess && result.Result is not null;
        }


        private async Task<bool> BeValidMeetupAsync(long meetupId, CancellationToken cancellationToken)
        {
            var result = await _meetupService.GetMeetupAsync(meetupId);
            return result.IsSuccess;
        }

    }
}
