using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.UnavailableDates
{
    record UnavailableDateDto(Guid Id, DateTime Date, string Reason);
}
