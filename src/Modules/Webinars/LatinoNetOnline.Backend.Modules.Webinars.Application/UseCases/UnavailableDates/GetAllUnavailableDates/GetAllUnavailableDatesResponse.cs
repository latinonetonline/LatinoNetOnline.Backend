namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.GetAllUnavailableDates
{
    public record GetAllUnavailableDatesResponse(Guid Id, DateTime Date, string Reason);
}