namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.CreateUnavailableDate
{
    public record CreateUnavailableDateResponse(Guid Id, DateTime Date, string Reason);
}