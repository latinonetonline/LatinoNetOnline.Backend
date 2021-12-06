using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.UnavailableDates
{
    record CreateUnavailableDateInput(DateTime Date, string Reason);

}
