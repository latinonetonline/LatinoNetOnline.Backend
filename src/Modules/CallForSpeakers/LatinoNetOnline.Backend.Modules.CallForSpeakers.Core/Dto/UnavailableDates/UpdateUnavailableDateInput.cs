using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.UnavailableDates
{
    record UpdateUnavailableDateInput(Guid Id, DateTime Date, string Reason);
}
