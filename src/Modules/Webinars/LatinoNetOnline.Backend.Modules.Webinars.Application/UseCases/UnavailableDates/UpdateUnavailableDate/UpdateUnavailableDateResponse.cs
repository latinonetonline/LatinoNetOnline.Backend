using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.UpdateUnavailableDate
{
    public record UpdateUnavailableDateResponse(Guid Id, DateTime Date, string Reason);
}