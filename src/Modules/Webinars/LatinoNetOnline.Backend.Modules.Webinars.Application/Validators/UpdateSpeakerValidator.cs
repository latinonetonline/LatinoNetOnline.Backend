using FluentValidation;

using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Speakers;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Validators
{
    class UpdateSpeakerValidator : AbstractValidator<UpdateSpeakerInput>
    {
        public UpdateSpeakerValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Ingrese un nombre.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Ingrese un apellido.");
            RuleFor(x => x.Image).NotEmpty().WithMessage("Ingrese un link de imagen.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Ingrese un email valido.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Ingrese una descipción personal.");
            RuleFor(x => x.Twitter).Must(BeAValidTwitter).WithMessage("Ingrese un usuario de Twitter valido.");
        }

        private bool BeAValidTwitter(string? twitter)
        {
            if (string.IsNullOrWhiteSpace(twitter))
                return true;

            if (twitter.StartsWith("@"))
            {
                twitter = twitter[1..];
            }


            return !string.IsNullOrWhiteSpace(twitter);
        }
    }
}
