using LatinoNetOnline.Backend.Shared.Commons.Results;

using MediatR;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.DeleteUnavailableDate
{
    public record DeleteUnavailableDateRequest(Guid Id) : IRequest<Result>;

}
