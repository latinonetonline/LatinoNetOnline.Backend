using LatinoNetOnline.Backend.Shared.Commons.Results;

using MediatR;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.GetAllUnavailableDates
{
    public record GetAllUnavailableDatesRequest() : IRequest<Result<GetAllUnavailableDatesResponse[]>>;

}
