using FluentValidation;

using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Services;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Validators
{
    class CreateWebinarValidator : AbstractValidator<CreateWebinarInput>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMeetupService  _meetupService;

        public CreateWebinarValidator(ApplicationDbContext dbContext, IMeetupService meetupService)
        {
            _dbContext = dbContext;
            _meetupService = meetupService;

            RuleFor(x => x.MeetupId).GreaterThan(0).WithMessage("Ingrese un Id de Meetup valido.");
            RuleFor(x => x.MeetupId).MustAsync(BeValidMeetupAsync).WithMessage("No existe un Meetup con ese Id.");

            RuleFor(x => x.ProposalId).MustAsync(BeExistProposalAsync).WithMessage("No existe una propuesta con ese Id.");

        }

        private Task<bool> BeExistProposalAsync(Guid proposalId, CancellationToken cancellationToken)
                => _dbContext.Proposals.AsNoTracking()
                    .AnyAsync(x => x.Id == proposalId && x.IsActive, cancellationToken);


        private async Task<bool> BeValidMeetupAsync(long meetupId, CancellationToken cancellationToken)
        {
            var result = await _meetupService.GetMeetupAsync(meetupId);
            return result.IsSuccess;
        }

    }
}
