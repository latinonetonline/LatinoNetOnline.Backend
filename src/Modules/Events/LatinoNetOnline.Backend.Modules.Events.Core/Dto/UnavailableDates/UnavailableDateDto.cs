using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.UnavailableDates
{
    record UnavailableDateDto(Guid Id, DateTime Date, string Reason);
}
