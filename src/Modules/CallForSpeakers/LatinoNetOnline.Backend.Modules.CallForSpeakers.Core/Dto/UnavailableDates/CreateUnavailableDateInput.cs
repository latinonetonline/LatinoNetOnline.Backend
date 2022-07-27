using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.UnavailableDates
{
    record CreateUnavailableDateInput(DateTime Date, string Reason);

}
