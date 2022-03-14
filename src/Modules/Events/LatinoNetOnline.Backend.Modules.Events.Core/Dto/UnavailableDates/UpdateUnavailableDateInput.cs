using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.UnavailableDates
{
    record UpdateUnavailableDateInput(Guid Id, DateTime Date, string Reason);
}
