using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.UnavailableDates
{
    record UnavailableDateDto(Guid Id, DateTime Date, string Reason);
}
