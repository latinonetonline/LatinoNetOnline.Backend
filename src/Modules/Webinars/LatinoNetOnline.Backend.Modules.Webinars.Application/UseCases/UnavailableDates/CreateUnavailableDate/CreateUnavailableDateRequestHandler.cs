using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.CreateUnavailableDate
{
    public class CreateUnavailableDateRequestHandler : IRequestHandler<CreateUnavailableDateRequest, Result<CreateUnavailableDateResponse>>
    {

        private readonly IRepository<UnavailableDate> _repository;

        public CreateUnavailableDateRequestHandler(IRepository<UnavailableDate> repository)
        {
            _repository = repository;
        }

        public async Task<Result<CreateUnavailableDateResponse>> Handle(CreateUnavailableDateRequest request, CancellationToken cancellationToken)
        {
            UnavailableDate entity = new(request.Date, request.Reason);

            await _repository.AddAsync(entity, cancellationToken);

            return new CreateUnavailableDateResponse(entity.Id, entity.Date, entity.Reason);
        }
    }
}
