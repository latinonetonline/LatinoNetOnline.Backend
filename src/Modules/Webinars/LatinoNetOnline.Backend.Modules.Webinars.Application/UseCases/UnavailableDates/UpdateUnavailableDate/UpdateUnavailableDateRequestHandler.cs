using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.UpdateUnavailableDate
{
    public class UpdateUnavailableDateRequestHandler : IRequestHandler<UpdateUnavailableDateRequest, Result<UpdateUnavailableDateResponse>>
    {
        private readonly IRepository<UnavailableDate> _repository;
        public UpdateUnavailableDateRequestHandler(IRepository<UnavailableDate> repository)
        {
            _repository = repository;
        }

        public async Task<Result<UpdateUnavailableDateResponse>> Handle(UpdateUnavailableDateRequest request, CancellationToken cancellationToken)
        {
            var entity = await _repository.SingleOrDefaultAsync(x => x.Id == request.Id, true, cancellationToken);


            if(entity is null)
            {
                return "No existe";
            }

            entity.Date = request.Date;
            entity.Reason = request.Reason;

            await _repository.UpdateAsync(entity, cancellationToken);

            return new UpdateUnavailableDateResponse(entity.Id, entity.Date, entity.Reason);
        }
    }
}
