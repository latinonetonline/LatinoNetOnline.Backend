using FluentValidation;

using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Validators;
using LatinoNetOnline.GenericRepository.Repositories;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.UpdateProposal
{
    public class UpdateProposalRequestValidator : AbstractValidator<UpdateProposalRequest>
    {
        private readonly IRepository<Proposal> _repository;

        public UpdateProposalRequestValidator(IRepository<Proposal> repository)
        {
            _repository = repository;

            RuleFor(x => x.Title).NotEmpty().WithMessage("Ingrese un título.");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Ingrese una descripción de su charla.");

            RuleFor(x => x.Date).Must(BeAValidSaturday).WithMessage("La fecha debe ser un Sábado.");

            RuleFor(x => x.Date).MustAsync(BeAValidDateAsync).WithMessage("Esa fecha ya esta ocupada. Escoja otra.");

            RuleFor(x => x.Speakers).NotEmpty().WithMessage("Debe especificar minimo un Spekaer.");

            RuleForEach(x => x.Speakers).SetValidator(new UpdateSpeakerValidator());

        }

        private bool BeAValidSaturday(DateTime date)
        {
            return date.DayOfWeek is DayOfWeek.Saturday;
        }

        private async Task<bool> BeAValidDateAsync(UpdateProposalRequest input, DateTime date, CancellationToken cancellationToken)
        {
            var existDate = await _repository.AnyAsync(x => x.EventDate.Date == date.Date && x.Id != input.Id && x.IsActive, false, cancellationToken);

            return !existDate;
        }
    }
}
