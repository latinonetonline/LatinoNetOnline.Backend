using FluentValidation;

using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Webinars;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Validators
{
    class CreateWebinarValidator : AbstractValidator<CreateWebinarInput>
    {
        private readonly ApplicationDbContext _dbContext;

        public CreateWebinarValidator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.YoutubeLink).NotNull().WithMessage("Ingrese un link de Youtube.");
            RuleFor(x => x.MeetupLink).NotNull().WithMessage("Ingrese un link de Meetup.");
            RuleFor(x => x.FlyerLink).NotNull().WithMessage("Ingrese un link del flyer.");

            RuleFor(x => x.ProposalId).MustAsync(BeExistProposalAsync).WithMessage("No existe una propuesta con ese Id.");

        }

        private Task<bool> BeExistProposalAsync(Guid proposalId, CancellationToken cancellationToken)
                => _dbContext.Proposals.AsNoTracking()
                    .AnyAsync(x => x.Id == proposalId && x.IsActive, cancellationToken);

    }
}
