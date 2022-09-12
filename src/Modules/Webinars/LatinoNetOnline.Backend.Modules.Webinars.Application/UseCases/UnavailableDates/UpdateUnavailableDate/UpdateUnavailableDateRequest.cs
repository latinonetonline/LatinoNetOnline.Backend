using LatinoNetOnline.Backend.Shared.Commons.Results;

using MediatR;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Webinars.Application.UseCases.UnavailableDates.UpdateUnavailableDate
{
    public record UpdateUnavailableDateRequest(Guid Id, DateTime Date, string Reason) : IRequest<Result<UpdateUnavailableDateResponse>>;

}
