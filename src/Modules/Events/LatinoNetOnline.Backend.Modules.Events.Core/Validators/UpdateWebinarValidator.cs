using FluentValidation;

using LatinoNetOnline.Backend.Modules.Events.Core.Dto.Webinars;
using LatinoNetOnline.Backend.Modules.Events.Core.Services;

using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Validators
{
    class UpdateWebinarValidator : AbstractValidator<UpdateWebinarInput>
    {
        private readonly IMeetupService _meetupService;

        public UpdateWebinarValidator(IMeetupService meetupService)
        {
            _meetupService = meetupService;

            RuleFor(x => x.MeetupId).GreaterThan(0).WithMessage("Ingrese un Id de Meetup valido.");
            RuleFor(x => x.MeetupId).MustAsync(BeValidMeetupAsync).WithMessage("No existe un Meetup con ese Id.");

            //RuleFor(x => x.Title).NotEmpty().WithMessage("Ingrese un Titulo valido.");
            //RuleFor(x => x.Description).NotEmpty().WithMessage("Ingrese una Descripción valida.");
            RuleFor(x => x.Status).IsInEnum().WithMessage("El estado no es valido.");

        }


        private async Task<bool> BeValidMeetupAsync(long meetupId, CancellationToken cancellationToken)
        {
            var result = await _meetupService.GetMeetupAsync(meetupId);
            return result.IsSuccess;
        }
    }
}
