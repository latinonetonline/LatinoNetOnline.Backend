using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Dto.UnavailableDates
{
    record CreateUnavailableDateInput(DateTime Date, string Reason);

}
