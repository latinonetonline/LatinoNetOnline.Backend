using FluentValidation;

using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Enums;

using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Validators
{
    class ConfirmWebinarValidator : AbstractValidator<Webinar>
    {
        public ConfirmWebinarValidator()
        {
            RuleFor(x => x.Flyer).NotNull();
            RuleFor(x => x.Streamyard).NotNull();
            RuleFor(x => x.LiveStreaming).NotNull();
            RuleFor(x => x.Status).Equal(WebinarStatus.Draft);
            RuleFor(x => x.StartDateTime).GreaterThanOrEqualTo(DateTime.Now);
        }
    }
}
