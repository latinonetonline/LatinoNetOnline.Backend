using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;
using LatinoNetOnline.Backend.Shared.Commons.Results;
using LatinoNetOnline.GenericRepository.Repositories;

using MediatR;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.GetAllUnavailableDates
{
    public class GetAllUnavailableDatesRequestHandler : IRequestHandler<GetAllUnavailableDatesRequest, Result<GetAllUnavailableDatesResponse[]>>
    {

        private readonly IRepository<UnavailableDate> _repository;

        public GetAllUnavailableDatesRequestHandler(IRepository<UnavailableDate> repository)
        {
            _repository = repository;
        }

        public async Task<Result<GetAllUnavailableDatesResponse[]>> Handle(GetAllUnavailableDatesRequest request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(false, cancellationToken);

            GetAllUnavailableDatesResponse[] response = entities.Select(x => new GetAllUnavailableDatesResponse(x.Id, x.Date, x.Reason)).ToArray();

            return response;
        }
    }
}
