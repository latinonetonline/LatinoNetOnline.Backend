using LatinoNetOnline.Backend.Shared.Commons.Results;

using MediatR;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.Proposals.UpdateWebinarNumbers
{
    public record UpdateWebinarNumbersRequest(string Name) : IRequest<Result>;

}
