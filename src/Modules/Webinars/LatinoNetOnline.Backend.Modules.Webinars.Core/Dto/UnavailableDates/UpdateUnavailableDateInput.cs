using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.UnavailableDates
{
    record UpdateUnavailableDateInput(Guid Id, DateTime Date, string Reason);
}
